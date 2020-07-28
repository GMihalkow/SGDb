<template>
    <div class="w-mx-60r p-1 mx-auto">
        <div class="p-card">
            <div class="p-card-body">
                <DataTable :value="creators" 
                        dataKey="id"
                        class="p-datatable-responsive"
                        columnResizeMode="fit"
                        :filters="filters"
                        :paginator="true" 
                        :loading="loading"
                        :rows="10"
                        :rowsPerPageOptions="[5,10,25,50]">
                    <template #header>
                        <div class="table-header">
                            List of Creators
                            <span class="p-input-icon-left">
                                <i class="pi pi-search" />
                                <InputText v-model="filters['global']" placeholder="Search by username" />
                            </span>
                        </div>
                    </template>
                     <template #empty>
                        No creators found.
                    </template>
                    <template #loading>
                        Loading creators data. Please wait.
                    </template>
                    <Column field="username" header="Username" :sortable="true" filterMatchMode="startsWith"/>
                    <Column field="totalGamesCreatedCount" header="Total Games Created" :sortable="true"/>
                    <Column field="createdOn" header="Created On" :sortable="true">
                        <template #body="slotProps">
                            <div>{{new Date(slotProps.data.createdOn).toLocaleString('en-US', { 
                                    year: 'numeric', 
                                    day:'numeric', 
                                    month: 'numeric'
                                })
                            }}</div>
                        </template>
                    </Column>
                    <Column header="Actions" headerStyle="text-align: center" bodyStyle="text-align: center; overflow: visible">
                        <template #body="slotProps">
                            <Button :disabled="currentCreatorId === slotProps.data.id" type="button" icon="pi pi-user-edit" v-on:click="onCreatorEdit(slotProps.data)" class="p-button-secondary"></Button>
                        </template>
                    </Column>
                </DataTable>

                <Toast />

                <Dialog :visible.sync="dialogVisible" header="Edit Creator" :modal="true">
                    <ValidationSummary v-bind:errors="errors"/>

                    <div class="p-cardialog-content">
                        <div class="p-grid p-fluid" v-if="creator">
                            <div class="p-col-12 mx-auto"><label for="username">Username</label></div>
                            <div class="p-col-12 mx-auto">
                                <InputText 
                                    id="username" 
                                    v-model.trim="form.username" 
                                    v-bind:class="{ 'p-invalid': !$v.form.username.required || !$v.form.username.maxLength }"
                                    @blur="setFieldValue('username', $event.target.value)"
                                    placeholder="Username"
                                    autocomplete="off" />
                                <ValidationMessages v-bind:validationContext="$v.form.username" v-bind:propName="'Username'" />
                            </div>
                        </div>
                    </div>

                    <template #footer>
                        <Button label="Save" @click="onEditSubmit" class="p-button-success p-button-text" />
                        <Button label="Close" @click="closeDialog" class="p-button-text" />
                    </template>
                </Dialog>
            </div>
        </div>
    </div>
</template>

<script>
    import DataTable from 'primevue/datatable';
    import Column from 'primevue/column';
    import InputText from 'primevue/inputtext';
    import Button from 'primevue/button';
    import Dialog from 'primevue/dialog';
    import Toast from 'primevue/toast';
    import ValidationMessages from '../../components/partials/ValidationMessages';
    import ValidationSummary from '../../components/partials/ValidationSummary';
    import creatorsApi from '../../api/creators/creators-api';
    
    import { required, maxLength } from 'vuelidate/lib/validators';
    import { mapGetters } from 'vuex';
    
    export default {
        name: 'CreatorsSearch',
        data() {
            return {
                creators: [],
                dialogVisible: false,
                creator: {},
                filters: {},
                form: {
                    id: '',
                    username: ''
                },
                loading: false,
                errors: []
            };
        },
        computed: {
            ...mapGetters({'currentCreatorId': 'creatorId'})
        },
        validations: {
            form: {
                username: {
                    required: required,
                    maxLength: maxLength(50)
                }
            }
        },
        components: {
            DataTable: DataTable,
            Column: Column,
            InputText: InputText,
            Dialog: Dialog,
            Button: Button,
            Toast: Toast,
            ValidationSummary: ValidationSummary,
            ValidationMessages: ValidationMessages
        },
        methods: {
            setFieldValue(fieldName, value) {
                this.form[fieldName] = value;
                this.$v.form[fieldName].$touch();
            },            
            onCreatorEdit(creator) {
                Object.assign(this.creator, creator);

                this.form.id = creator.id;
                this.form.username = creator.username;

                this.dialogVisible = true;
            },
            onEditSubmit() {
                var _this = this;

                if (!_this.$v.$invalid) {
                    creatorsApi.edit({ id: this.form.id, username: this.form.username }).then(function(res){
                        _this.reloadData();
                        _this.$toast.add({severity: 'success', summary: 'Creator Updated.', life: 3000});

                        _this.creator.username = _this.form.username;
                        _this.$set(_this.creators, _this.creators.findIndex(function(cr) { return cr.id === _this.creator.id; }), _this.creator);
                    }).catch(function(err) {
                        _this.$toast.add({severity: 'error', summary: 'Something went wrong.', life: 3000});

                        _this.$set(_this.creators, _this.creators.findIndex(function(cr) { return cr.id === _this.creator.id; }), _this.creator);
                    });

                    this.closeDialog();
                }
            },
            closeDialog() {
                this.dialogVisible = false;
            },
            reloadData() {
                var _this = this;
                _this.loading = true;
                _this.creators = [];

                creatorsApi.getAll().then(function(res) {
                    _this.loading = false;
                    _this.creators = res.data.data;
                }).catch(function(err) {
                    _this.loading = false;
                    console.log(err);
                });
            }
        },
        created() {
            this.reloadData();
        }
    };
</script>

<style scoped></style>