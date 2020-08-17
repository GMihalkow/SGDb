<template>
    <div class="w-mx-60r p-1 mx-auto">
        <div class="p-card">
            <div class="p-card-body">
                <DataTable :value="genres" 
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
                                List of Genres
                                <span class="p-input-icon-left">
                                    <i class="pi pi-search" />
                                    <InputText v-model="filters['global']" placeholder="Search by name" />
                                </span>
                            </div>
                            <div class="p-lg-2 p-sm-2 p-xs-12 text-start p-0">
                                <div class="ml-auto w-min-content">
                                    <Button label="Create" class="p-button-primary" @click="openGenreCreateDialog" />
                                </div>
                            </div>
                        </div>
                    </template>
                    <template #empty>
                        No genres found.
                    </template>
                    <template #loading>
                        Loading genres data. Please wait.
                    </template>
                    <Column field="name" header="Name" :sortable="true" filterMatchMode="startsWith"/>
                    <Column field="creatorName" header="Creator" :sortable="true"/>
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
                            <div v-if="currentCreatorId === slotProps.data.creatorId || isUserAdmin">
                                <Button type="button" icon="pi pi-user-edit" @click="openGenreEditDialog(slotProps.data)" class="p-button-secondary p-mr-2"></Button>
                                <Button type="button" icon="pi pi-trash" class="p-button-danger" @click="openDeleteGenreDialog(slotProps.data)"/>
                            </div>
                        </template>
                    </Column>
                </DataTable>

                <Toast />

                <Dialog :visible.sync="createGenreDialog" 
                    :header="isEdit ? 'Edit Genre' : 'Create Genre'"
                    :modal="true">
                    <ValidationSummary v-bind:errors="errors"/>
                    <div class="p-cardialog-content">
                        <div class="p-grid p-fluid" v-if="genre">
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
                        <Button v-if="!isEdit" label="Create" icon="pi pi-check" class="p-button-success p-button-text" @click="confirmCreateGenre" />
                        <Button v-else label="Update" icon="pi pi-check" class="p-button-success p-button-text" @click="confirmEditGenre" /> 
                        <Button label="Close" @click="closeDialogs" class="p-button-text" />
                    </template>
                </Dialog>

                <Dialog header="Delete Genre" :visible.sync="deleteGenreDialog" :style="{width: '350px'}" :modal="true">
                    <div class="confirmation-content">
                        <i class="pi pi-exclamation-triangle p-mr-3" style="font-size: 2rem" />
                        <span>Are you sure you want to delete this genre?</span>
                    </div>
                    <template #footer>
                        <Button label="Yes" icon="pi pi-check" @click="confirmDeleteGenre" class="p-button-success p-button-text" autofocus />
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
    import genresApi from '../../api/creators/genres-api';

    import { required, maxLength } from 'vuelidate/lib/validators';
    import { mapGetters } from 'vuex';

    export default {
        name: 'GenresSearch',
        data() {
            return {
                genres: [],
                errors: [],
                genre: {},
                filters: {},
                form: {
                    id: '',
                    name: ''
                },
                deleteGenreDialog: false,
                createGenreDialog: false,
                isEdit: false,
                loading: false
            };
        },
        computed: {
            ...mapGetters({'currentCreatorId': 'creatorId'}),
            ...mapGetters({'isUserAdmin': 'isUserAdmin'})
        },
        validations: {
            form: {
                name: {
                    required: required,
                    maxLength: maxLength(500)
                }
            }
        },
        components: {
            DataTable: DataTable,
            Dialog: Dialog,
            Column: Column,
            Button: Button,
            Toast: Toast,
            InputText: InputText,
            ValidationSummary: ValidationSummary,
            ValidationMessages: ValidationMessages
        },
        methods: {
            openDeleteGenreDialog(genre) {
                this.deleteGenreDialog = true;
                Object.assign(this.genre, genre);
            },
            openGenreCreateDialog() {
                this.isEdit = false
                this.createGenreDialog = true;
                this.form.name = '';
            },
            openGenreEditDialog(genre) {
                this.isEdit = true;
                this.createGenreDialog = true;
                Object.assign(this.genre, genre);
                Object.assign(this.form, genre);
            },
            confirmEditGenre() {
                var _this = this;
                
                if (!_this.$v.$invalid) {
                    _this.loading = true;

                    var genreObj = { id: _this.form.id, name: _this.form.name };

                    genresApi.edit(genreObj).then(function() {
                        _this.loading = false;
                        _this.$toast.add({severity: 'success', summary: 'Genre Edited.', life: 3000});                   
                        _this.form.name = '';
                        _this.reloadData();
                    }).catch(function(err) {
                        _this.loading = false;
                        
                        if (err.response) {
                            var data = err.response.data;

                            if (!data.succeeded) {
                                _this.$toast.add({severity: 'error', summary: data.errors[0], life: 3000});
                            }
                        } else {
                            _this.$toast.add({severity: 'error', summary: 'Something went wrong.', life: 3000});
                        }
                    }); 
                    
                    _this.closeDialogs();
                } else {
                    _this.$v.$touch();
                }
            },
            confirmCreateGenre() {
                var _this = this;

                if (!_this.$v.$invalid) {
                    _this.loading = true;

                    var formObj = new FormData();
                    formObj.append('Name', _this.form.name);

                    genresApi.create(formObj).then(function(res) {
                        _this.loading = false;
                        _this.$toast.add({severity: 'success', summary: 'Genre Created.', life: 3000});                   
                        _this.form.name = '';
                        _this.reloadData();
                    }).catch(function(err) {
                        _this.loading = false;
                        
                        if (err.response) {
                            var data = err.response.data;

                            if (!data.succeeded) {
                                _this.$toast.add({severity: 'error', summary: data.errors[0], life: 3000});
                            }
                        } else {
                            _this.$toast.add({severity: 'error', summary: 'Something went wrong.', life: 3000});
                        }
                    });

                    _this.closeDialogs();
                } else {
                    _this.$v.$touch();
                }
            },
            confirmDeleteGenre() {
                var _this = this;
                _this.loading = true;

                genresApi.delete({ id: this.genre.id }).then(function(res) {
                    _this.loading = false;
                    
                    _this.$toast.add({severity: 'success', summary: 'Genre Deleted.', life: 3000});
                    
                    _this.publisher = {};
                    _this.reloadData();
                }).catch(function(err) {
                    _this.loading = false;
                    _this.$toast.add({severity: 'error', summary: 'Something went wrong.', life: 3000});

                    _this.publisher = {};
                });

                _this.closeDialogs();
            },
            closeDialogs() {
                this.deleteGenreDialog = false;
                this.createGenreDialog = false;
            },
            reloadData() {
                var _this = this;

                _this.loading = true;
                _this.genres = [];

                genresApi.getAllSearchGenres().then(function(res) {
                    _this.loading = false;
                    
                    var genres = res.data.data;
                    _this.genres = genres;
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