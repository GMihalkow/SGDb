<template>
        <div class="w-mx-60r p-1 mx-auto">
            <div class="p-card">
                <div class="p-card-body">
                    <DataTable :value="publishers" 
                            dataKey="id"
                            class="p-datatable-responsive"
                            columnResizeMode="fit"
                            :filters="filters"
                            :paginator="true" 
                            :loading="loading"
                            :rows="10"
                            :rowsPerPageOptions="[5,10,25,50]">
                        <template #header>
                            <div class="p-grid p-nogutter">
                                <div class="p-lg-10 p-sm-10 p-xs-12 text-start p-0">
                                    List of Publishers
                                    <span class="p-input-icon-left">
                                        <i class="pi pi-search" />
                                        <InputText v-model="filters['global']" placeholder="Search by name" />
                                    </span>
                                </div>
                                <div class="p-lg-2 p-sm-2 p-xs-12 text-start p-0">
                                    <div class="ml-auto w-min-content">
                                        <Button label="Create" class="p-button-primary" @click="openPublisherCreateDialog" />
                                    </div>
                                </div>
                            </div>
                        </template>
                        <template #empty>
                            No publishers found.
                        </template>
                        <template #loading>
                            Loading creators data. Please wait.
                        </template>
                        <Column field="name" header="Name" :sortable="true" filterMatchMode="startsWith"/>
                        <Column field="creatorName" header="Creator" :sortable="true"/>
                        <Column field="createdOn" header="Created On" :sortable="true">
                            <template #body="slotProps">
                                <div>{{new Date(slotProps.data.createdOn).toLocaleDateString()}}</div>
                            </template>
                        </Column>
                        <Column header="Actions" headerStyle="text-align: center" bodyStyle="text-align: center; overflow: visible">
                            <template #body="slotProps">
                                <div v-if="currentCreatorId === slotProps.data.creatorId || isUserAdmin">
                                    <Button type="button" icon="pi pi-user-edit" @click="openPublisherEditDialog(slotProps.data)" class="p-button-secondary p-mr-2"></Button>
                                    <Button type="button" icon="pi pi-trash" class="p-button-danger" @click="openDeletePublisherDialog(slotProps.data)"/>
                                </div>
                            </template>
                        </Column>
                    </DataTable>

                    <Toast />

                    <Dialog :visible.sync="createPublisherDialog" 
                        :header="isEdit ? 'Edit Publisher' : 'Create Publisher'"
                        :modal="true">
                        <ValidationSummary v-bind:errors="errors"/>
                        <div class="p-cardialog-content">
                            <div class="p-grid p-fluid" v-if="publisher">
                                <div class="p-col-12 mx-auto"><label for="name">Name</label></div>
                                <div class="p-col-12 mx-auto">
                                    <InputText 
                                        id="name"
                                        v-model="form.name" 
                                        v-bind:class="{ 'p-invalid': !$v.form.name.required || !$v.form.name.maxLength }"
                                        @blur="setFieldValue('name', $event.target.value)"
                                        placeholder="Name" />
                                    <ValidationMessages v-bind:validationContext="$v.form.name" v-bind:propName="'Name'" />
                                </div>
                            </div>
                        </div>
                        <template #footer>
                            <Button v-if="!isEdit" label="Create" icon="pi pi-check" class="p-button-success p-button-text" @click="confirmCreatePublisher" />
                            <Button v-else label="Update" icon="pi pi-check" class="p-button-success p-button-text" @click="confirmEditPublisher" /> 
                            <Button label="Close" @click="closeDialogs" class="p-button-text" />
                        </template>
                    </Dialog>

                    <Dialog header="Delete Publisher" :visible.sync="deletePublisherDialog" :style="{width: '350px'}" :modal="true">
                        <div class="confirmation-content">
                            <i class="pi pi-exclamation-triangle p-mr-3" style="font-size: 2rem" />
                            <span>Are you sure you want to delete this publisher?</span>
                        </div>
                        <template #footer>
                            <Button label="Yes" icon="pi pi-check" @click="confirmDeletePublisher" class="p-button-success p-button-text" autofocus />
                            <Button label="No" icon="pi pi-times" @click="closeDialogs" class="p-button-text" />
                        </template>
                    </Dialog>
                </div>
            </div>
        </div>
</template>

<script>
    import DataTable from 'primevue/datatable';
    import Dialog from 'primevue/dialog';
    import Column from 'primevue/column';
    import Button from 'primevue/button';
    import Toast from 'primevue/toast';
    import InputText from 'primevue/inputtext';
    import ValidationSummary from '../../components/partials/ValidationSummary';
    import ValidationMessages from '../../components/partials/ValidationMessages';
    import publishersApi from '../../api/creators/publishers-api';

    import { required, maxLength } from 'vuelidate/lib/validators';
    import { mapGetters } from 'vuex';

    export default {
        name: 'PublishersSearch',
        data() {
            return {
                publishers: [],
                errors: [],
                publisher: {},
                filters: {},
                form: {
                    name: ''
                },
                deletePublisherDialog: false,
                createPublisherDialog: false,
                isEdit: false,
                loading: false
            }
        },
        computed: {
            ...mapGetters({'currentCreatorId': 'creatorId'}),
            ...mapGetters({'isUserAdmin': 'isUserAdmin'})
        },
        components: {
            DataTable: DataTable,
            Dialog: Dialog,
            Column: Column,
            Toast: Toast,
            Button: Button,
            ValidationMessages: ValidationMessages,
            ValidationSummary: ValidationSummary,
            InputText: InputText
        },
        validations: {
            form: {
                name: {
                    required: required,
                    maxLength: maxLength(500)
                }
            }
        },
        methods: {
            openDeletePublisherDialog(publisher) {
                this.deletePublisherDialog = true;
                Object.assign(this.publisher, publisher);
            },
            openPublisherCreateDialog() {
                this.isEdit = false
                this.createPublisherDialog = true;
                this.form.name = '';
            },
            openPublisherEditDialog(publisher) {
                this.isEdit = true;
                this.createPublisherDialog = true;
                Object.assign(this.publisher, publisher);
                Object.assign(this.form, publisher);
            },
            confirmEditPublisher() {
                var _this = this;
                _this.loading = true;

                publishersApi.edit({id: _this.form.id, name: _this.form.name}).then(function(res) {
                    _this.loading = false;
                    _this.$toast.add({severity: 'success', summary: 'Publisher Updated.', life: 3000});
                    _this.form.name = '';
                    _this.reloadData();
                }).catch(function(err) {
                    _this.loading = false;

                    if (err.response) {
                        var data = err.response.data;

                        if (!data.succeeded) {
                            _this.$toast.add({severity: 'error', summary: data.errors[0] ? data.errors[0] : 'Something went wrong.', life: 3000});
                        }
                    } else {
                        _this.$toast.add({severity: 'error', summary: 'Something went wrong.', life: 3000});
                    }

                    _this.form.name = '';
                });

                _this.closeDialogs();
            },
            confirmCreatePublisher() {
                var _this = this;
                _this.loading = true;

                var formObj = new FormData();
                formObj.append('Name', _this.form.name);

                publishersApi.create(formObj).then(function(res) {
                    _this.loading = false;
                    _this.$toast.add({severity: 'success', summary: 'Publisher Created.', life: 3000});                   
                    _this.form.name = '';
                    _this.reloadData();
                }).catch(function(err) {
                    _this.loading = false;

                    if (err.response){
                        var data = err.response.data;

                        if (!data.succeeded) {
                            _this.errors = data.errors;
                        }
                    } else {
                        _this.errors = ['Something went wrong.'];
                    }
                });

                _this.closeDialogs();
            },
            confirmDeletePublisher() {
                var _this = this;
                _this.loading = true;

                publishersApi.delete({id: _this.publisher.id}).then(function(res) {
                    _this.loading = false;
                    
                    _this.$toast.add({severity: 'success', summary: 'Publisher Deleted.', life: 3000});
                    
                    _this.publisher = {};
                    _this.deletePublisherDialog = false;
                    _this.reloadData();
                }).catch(function(err) {
                    _this.loading = false;
                    _this.$toast.add({severity: 'error', summary: 'Something went wrong.', life: 3000});

                    _this.publisher = {};
                    _this.deletePublisherDialog = false;
                });

                _this.closeDialogs();
            },
            closeDialogs() {
                this.deletePublisherDialog = false;
                this.createPublisherDialog = false;
            },
            reloadData() {
                var _this = this;

                _this.loading = true;
                _this.publishers = [];

                publishersApi.getAllSearchPublishers().then(function(res) {
                    _this.loading = false;
                    
                    var publishers = res.data.data;
                    _this.publishers = publishers;
                }).catch(function(err) {
                    _this.loading = false;
                    _this.$toast.add({severity: 'error', summary: 'Something went wrong.', life: 3000});
                });
            },
            setFieldValue(fieldName, value) {
                this.form[fieldName] = value;
                this.$v.form[fieldName].$touch();
            }
        },
        created() {
            this.reloadData();
        }
    }
</script>

<style scoped></style>