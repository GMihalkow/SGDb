<template>
    <div class="w-mx-60r p-1 mx-auto">
        <div class="p-card">
            <div class="p-card-body">
                <DataTable :value="roles" 
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
                                List of Roles
                                <span class="p-input-icon-left">
                                    <i class="pi pi-search" />
                                    <InputText v-model="filters['global']" placeholder="Search by name" />
                                </span>
                            </div>
                            <div class="p-lg-2 p-sm-2 p-xs-12 text-start p-0">
                                <div class="ml-auto w-min-content">
                                    <Button label="Create" @click="openCreateRoleDialog" class="p-button-primary"/>
                                </div>
                            </div>
                        </div>
                    </template>
                     <template #empty>
                        No roles found.
                    </template>
                    <template #loading>
                        Loading roles data. Please wait.
                    </template>
                    <Column field="name" header="Name" :sortable="true" filterMatchMode="contains"/>
                    <Column header="Actions" headerStyle="text-align: center" bodyStyle="text-align: center; overflow: visible">
                        <template #body="slotProps">
                            <div>
                                <Button type="button" icon="pi pi-user-edit" class="p-button-secondary p-mr-2" @click="openEditRoleDialog(slotProps.data)"></Button>
                                <Button type="button" icon="pi pi-trash" class="p-button-danger" @click="openDeleteRoleDialog(slotProps.data)"/>
                            </div>
                        </template>
                    </Column>
                </DataTable>

                <Toast />

                <Dialog :visible.sync="createRoleDialog" :header="isEdit ? 'Edit Role' : 'Create Role'" :modal="true">
                    <ValidationSummary v-bind:errors="errors"/>

                    <div class="p-cardialog-content">
                        <div class="p-grid p-fluid" v-if="role">
                            <div class="p-col-12 mx-auto"><label for="role">Role</label></div>
                            <div class="p-col-12 mx-auto">
                                <InputText 
                                    id="role" 
                                    v-model.trim="form.name" 
                                    v-bind:class="{ 'p-invalid': !$v.form.name.required || !$v.form.name.maxLength || !$v.form.name.minLength }"
                                    @blur="setFieldValue('name', $event.target.value)"
                                    placeholder="Role"
                                    autocomplete="off" />
                                <ValidationMessages v-bind:validationContext="$v.form.name" v-bind:propName="'Role'" />
                            </div>
                        </div>
                    </div>

                    <template #footer>
                        <Button v-if="!isEdit" label="Create" icon="pi pi-check" class="p-button-success p-button-text" @click="confirmCreateRole" />
                        <Button v-else label="Update" icon="pi pi-check" class="p-button-success p-button-text" @click="confirmEditRole" />
                        <Button label="Close" @click="closeDialogs" class="p-button-text" />
                    </template>
                </Dialog>

                <Dialog header="Delete Role" :visible.sync="deleteRoleDialog" :style="{width: '350px'}" :modal="true">
                    <div class="confirmation-content">
                        <i class="pi pi-exclamation-triangle p-mr-3" style="font-size: 2rem" />
                        <span>Are you sure you want to delete this role?</span>
                    </div>
                    <template #footer>
                        <Button label="Yes" icon="pi pi-check" @click="confirmDeleteRole" class="p-button-success p-button-text" autofocus />
                        <Button label="No" icon="pi pi-times" @click="closeDialogs" class="p-button-text" />
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
    import rolesApi from '../../api/identity/roles-api';
    
    import { required, maxLength, minLength } from 'vuelidate/lib/validators';

    export default {
        name: 'RolesSearch',
        data() {
            return {
                roles: [],
                role: {},
                filters: {},
                form: {
                    name: ''
                },
                loading: false,
                createRoleDialog: false,
                deleteRoleDialog: false,
                isEdit: false,
                errors: []
            };
        },
        validations: {
            form: {
                name: {
                    required: required,
                    minLength: minLength(4),
                    maxLength: maxLength(256)
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
            closeDialogs() {
                this.createRoleDialog = false;
                this.deleteRoleDialog = false;
            },
            openEditRoleDialog(role) {
                this.isEdit = true;
                this.createRoleDialog = true;
                Object.assign(this.role, role);
                Object.assign(this.form, role);
            },
            openDeleteRoleDialog(role) {
                this.isEdit = false;
                this.deleteRoleDialog = true;
                Object.assign(this.role, role);
            },
            openCreateRoleDialog() {
                this.isEdit = false;
                this.form.name = '';
                this.createRoleDialog = true;
            },
            confirmEditRole() {
                var _this = this;

                if (!_this.$v.$invalid) {
                    _this.loading = true;

                    rolesApi.edit(_this.form).then(function(res) {
                        _this.loading = false;
                        _this.$toast.add({severity: 'success', summary: 'Role Edited.', life: 3000});
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
                    })

                    _this.closeDialogs();
                }
            },
            confirmCreateRole() {
                var _this = this;

                if (!_this.$v.$invalid) {
                    _this.loading = true;

                    var formObj = new FormData();
                    formObj.append('Name', _this.form.name);

                    rolesApi.create(formObj).then(function(res) {
                        _this.loading = false;
                        _this.$toast.add({severity: 'success', summary: 'Role Created.', life: 3000});
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
                    })

                    _this.closeDialogs();
                }
            },
            confirmDeleteRole() {
                var _this = this;

                _this.loading = true;

                rolesApi.delete({ id: this.role.id }).then(function(res) {
                    _this.loading = false;
                    _this.$toast.add({severity: 'success', summary: 'Role Deleted.', life: 3000});
                    _this.role = {};
                    _this.reloadData();
                }).catch(function(err) {
                    _this.loading = false;
                    _this.role = {};
                        
                    if (err.response) {
                        var data = err.response.data;

                        if (!data.succeeded) {
                            _this.$toast.add({severity: 'error', summary: data.errors[0], life: 3000});
                        }
                    } else {
                        _this.$toast.add({severity: 'error', summary: 'Something went wrong.', life: 3000});
                    }
                });

                this.closeDialogs();
            },
            reloadData() {
                var _this = this;
                _this.loading = true;
                _this.creators = [];

                rolesApi.getAll().then(function(res) {
                    _this.loading = false;
                    _this.roles = res.data.data;
                }).catch(function(err) {
                    _this.loading = false;
                });
            }
        },
        created() {
            this.reloadData();
        }
    };
</script>

<style scoped></style>