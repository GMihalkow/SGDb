<template>
    <div class="w-mx-60r p-1 mx-auto">
        <div class="p-card">
            <div class="p-card-body">
                <DataTable :value="viewedGames" 
                        dataKey="id"
                        class="p-datatable-responsive"
                        columnResizeMode="fit"
                        :paginator="true" 
                        :filters="filters"
                        :loading="loading"
                        :rows="10"
                        :rowsPerPageOptions="[5,10,25,50]">
                    <template #header>
                        <div class="table-header">
                            History of viewed games
                            <span class="p-input-icon-left">
                                <i class="pi pi-search" />
                                <InputText v-model="filters['game']" placeholder="Search by name" />
                            </span>
                        </div>
                    </template>
                     <template #empty>
                        No viewed games found.
                    </template>
                    <template #loading>
                        Loading viewed games data. Please wait.
                    </template>
                    <Column field="game" header="Name" :sortable="true" filterMatchMode="custom" :filterFunction="filterNames">
                        <template #body="slotProps">
                            <div>{{slotProps.data.game ? slotProps.data.game.name : '-'}}</div>
                        </template>
                    </Column>
                    <Column field="createdOn" header="Created On" :sortable="true">
                        <template #body="slotProps">
                            <div>{{new Date(slotProps.data.createdOn).toLocaleString('en-US', { 
                                    year: 'numeric', 
                                    day:'numeric', 
                                    month: 'numeric', 
                                    hour: 'numeric', 
                                    minute: 'numeric', 
                                    hour12: true
                                })
                            }}</div>
                        </template>
                    </Column>
                    <Column header="Actions" headerStyle="text-align: center" bodyStyle="text-align: center; overflow: visible">
                        <template #body="slotProps">
                            <Button type="button"
                                label="Details"
                                icon="pi pi-check-square" 
                                v-on:click="redirectToGameDetails(slotProps.data.gameId)"></Button>
                        </template>
                    </Column>
                </DataTable>

                <Toast />
            </div>
        </div>
    </div>
</template>

<script>
    import DataTable from 'primevue/datatable';
    import Column from 'primevue/column';
    import Toast from 'primevue/toast';
    import InputText from 'primevue/inputtext';
    import Button from 'primevue/button';
    import creatorsGatewayApi from '../../api/creators-gateway/creators-api-gateway';
    
    export default {
        name: 'UserViewedGamesHistory',
        data() {
            return {
                loading: true,
                viewedGames: [],
                filters: {},
                errors: []
            }
        },
        components: {
            DataTable: DataTable,
            Column: Column,
            Toast: Toast,
            InputText: InputText,
            Button: Button
        },
        methods: {
            filterNames(game, filterValue) {
                if (!game) { return false; }

                return game.name.toLowerCase().indexOf(filterValue.toLowerCase()) === 0;
            },
            redirectToGameDetails(gameId) {
                this.$router.push({ name: 'gameDetails', params: { id: gameId } });
            }
        },
        created() {
            var _this = this;

            creatorsGatewayApi.getMyViewedGamesHistory().then(function(res) {
                var data = res.data.data;
                _this.viewedGames = data;
                
                _this.loading = false;
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
        }
    }
</script>

<style scoped></style>