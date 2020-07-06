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
                        :rows="10"
                        :rowsPerPageOptions="[5,10,25,50]">
                    <template #header>
                        <div class="table-header">
                            List of Creators
                            <span class="p-input-icon-left">
                                <i class="pi pi-search" />
                                <InputText v-model="filters['global']" placeholder="Search by Username" />
                            </span>
                        </div>
                    </template>
                     <template #empty>
                        No creators found.
                    </template>
                    <template #loading>
                        Loading creators data. Please wait.
                    </template>
                    <Column class="text-center" field="username" header="Username" :sortable="true" filterMatchMode="startsWith"></Column>
                    <Column field="createdOn" header="Created On" :sortable="true">
                        <template #body="slotProps">
                            <div>{{new Date(slotProps.data.createdOn).toLocaleDateString()}}</div>
                        </template>
                    </Column>
                    <Column field="totalGamesCreatedCount" header="Total Games Created" :sortable="true"></Column>
                    <Column header="Actions" headerStyle="text-align: center" bodyStyle="text-align: center; overflow: visible">
                        <template #body="slotProps">
                            <Button type="button" icon="pi pi-user-edit" v-on:click="onCreatorEdit(slotProps.data)" class="p-button-secondary"></Button>
                        </template>
                    </Column>
                </DataTable>

                <Toast />

                <Dialog :visible.sync="dialogVisible" header="Creator Details" :modal="true">
                    <div class="p-cardialog-content">
                        <div class="p-grid p-fluid" v-if="creator">
                            <div class="p-col-12 mx-auto"><label for="username">Username</label></div>
                            <div class="p-col-12 mx-auto">
                                <InputText id="username" v-model="form.username" autocomplete="off" />
                            </div>
                        </div>
                    </div>

                    <template #footer>
                        <Button label="Save" @click="onEditSubmit" class="p-button-success" />
                        <Button label="Close" @click="closeDialog" class="p-button-secondary" />
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
    import creatorsApi from '../../api/creators/creators-api';
    
    export default {
        name: 'CreatorsSearch',
        data() {
            return {
                creators: [],
                dialogVisible: false,
                creator: {},
                filters: {},
                form: {}
            };
        },
        components: {
            DataTable: DataTable,
            Column: Column,
            InputText: InputText,
            Dialog: Dialog,
            Button: Button,
            Toast: Toast
        },
        methods: {
            onCreatorEdit(creator) {
                this.creator = creator;
                Object.assign(this.form, creator);

                this.dialogVisible = true;
            },
            onEditSubmit() {
                var _this = this;

                creatorsApi.edit({ id: this.form.id, username: this.form.username }).then(function(res){
                    _this.$toast.add({severity: 'success', summary: 'Creator Updated.'});
                    
                    _this.creator.username = _this.form.username;
                }).catch(function(err) {
                    _this.$toast.add({severity: 'error', summary: 'Error occured.'});
                });

                this.closeDialog();
            },
            closeDialog() {
                this.dialogVisible = false;
            }
        },
        mounted() {
            var _this = this;

            creatorsApi.getAll().then(function(res) {
                _this.creators = res.data.data;
            }).catch(function(err) {
                console.log(err);
            });
        }
    };
</script>

<style scoped>

</style>