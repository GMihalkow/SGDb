<template>
    <div class="w-mx-60r p-1 m1r-auto">
        <h1 class="text-center mb-1">Featured Games</h1>
        <DataView 
            :rows="9"
            :layout="layout"
            :paginator="true" 
            :sortOrder="sortOrder" 
            :sortField="sortField" 
            :value="featuredGames" 
            :rowsPerPageOptions="[9,18,27]">
            <template #header>
                <div class="p-grid p-nogutter">
                    <div class="p-lg-4 p-sm-4 p-xs-12 text-start p-0">
                        <Dropdown v-model="sortKey" :options="sortOptions" optionLabel="label" placeholder="Sort By" @change="onSortChange($event)"/>
                    </div>
                    <div class="p-lg-4 p-sm-4 p-xs-12 text-center p-0">
                        <InputText v-model="searchFilter" placeholder="Search by name..."/>
                    </div>
                    <div class="p-lg-4 p-sm-4 p-xs-12 text-end p-0">
                        <DataViewLayoutOptions v-model="layout"/>
                    </div>
                </div>
            </template>
            <template #list="slotProps">
                <div class="p-col-12">
                    <div class="game-data-list-item">
                        <img :src="slotProps.data.headerImageUrl" :alt="slotProps.data.name"/>
                        <div class="p-1">
                            <div>Name: <b>{{slotProps.data.name}}</b></div>
                            <div>Publishers: <b>{{slotProps.data.publisherNames.length > 50 ? slotProps.data.publisherNames.substr(0, 50) + '...' : slotProps.data.publisherNames}}</b></div>
                            <div>Genres: <b>{{slotProps.data.genreNames.length > 50 ? slotProps.data.genreNames.substr(0, 50) + '...' : slotProps.data.genreNames}}</b></div>
                            <div>Released On: <b>{{slotProps.data.releasedOn ? new Date(slotProps.data.releasedOn).toLocaleString('en-US', { 
                                    year: 'numeric', 
                                    day:'numeric', 
                                    month: 'numeric'
                                }) : 'TBD'}}</b></div>
                            <Button label="Details" icon="pi pi-search" @click="gameDetails(slotProps.data.id)"></Button>
                        </div>
                    </div>
                </div>
            </template>
            <template #grid="slotProps">
                <div class="p-col-12 p-md-4 mb-1">
                    <div class="game-grid-item card">
                        <div class="game-grid-item-content text-start">
                            <img :src="slotProps.data.headerImageUrl" :alt="slotProps.data.name"/>
                            <div>
                                <b>{{slotProps.data.name.length > 15 ? slotProps.data.name.substr(0, 15) + '...' : slotProps.data.name}}</b>
                            </div>
                            <div>Release Date: 
                                <b>{{slotProps.data.releasedOn ? new Date(slotProps.data.releasedOn).toLocaleString('en-US', { 
                                    year: 'numeric', 
                                    day:'numeric', 
                                    month: 'numeric'
                                }) : 'TBD'}}</b>
                            </div>
                            <div>Publishers: 
                                <b v-if="slotProps.data.publisherNames.length">{{slotProps.data.publisherNames.length > 20 ? slotProps.data.publisherNames.substr(0, 20) + '...' : slotProps.data.publisherNames}}</b>
                                <b v-else>-</b>
                            </div>
                            <div>Genres: 
                                <b v-if="slotProps.data.genreNames.length">{{slotProps.data.genreNames.length > 20 ? slotProps.data.genreNames.substr(0, 20) + '...' : slotProps.data.genreNames}}</b>
                                <b v-else>-</b>
                            </div>
                        </div>
                        <div class="game-grid-item-bottom text-end">
                            <Button label="Details" icon="pi pi-search" @click="gameDetails(slotProps.data.id)"></Button>
                        </div>
                    </div>
                </div>
            </template>
            <template #empty><div class="p-1">No games found.</div></template>
        </DataView>
    </div>
</template>

<script>
    import DataView from 'primevue/dataview';
    import Button from 'primevue/button';
    import Dropdown from 'primevue/dropdown';
    import InputText from 'primevue/inputtext';
    import DataViewLayoutOptions from 'primevue/dataviewlayoutoptions';
    import gamesApi from '../../api/creators/games-api';

    export default {
        name: 'FeaturedGames',
        components: {
            DataView: DataView,
            Dropdown: Dropdown,
            InputText: InputText,
            DataViewLayoutOptions: DataViewLayoutOptions,
            Button: Button
        },
        data() {
            return {
                featuredGames: [],
                layout: 'grid',
                searchFilter: '',
                sortKey: null,
                sortOrder: null,
                sortField: null,
                sortOptions: [
                    {label: 'Latest First', value: '!releasedOn'},
                    {label: 'Oldest First', value: 'releasedOn'},
                    {label: 'Name', value: 'name'}
                ]
            };
        },
        methods: {
            onSortChange(e) {
                const sortKey = e.value;
                const value = e.value.value;
                
                if (value.indexOf('!') === 0) {
                    this.sortOrder = -1;
                    this.sortField = value.substring(1);
                    this.sortKey = sortKey;
                }
                else {
                    this.sortOrder = 1;
                    this.sortField = value;
                    this.sortKey = sortKey;
                }
            },
            gameDetails(id) {
                this.$router.push({ name: 'gameDetails', params: { id: id } });
            },
            reloadData() {
                var _this = this;
                _this.featuredGames = [];

                gamesApi.getAllFeatured().then(function(res) {
                    var data = res.data.data;

                    var lowerCasedSearchFilter = _this.searchFilter.toLowerCase();
                    
                    _this.featuredGames = data.filter(function(game) { return game.name.toLowerCase().indexOf(lowerCasedSearchFilter) >= 0; });
                }).catch(function(err) {
                    console.log(err);
                });
            }
        },
        watch: {
            searchFilter() {
                this.reloadData();
            }
        },
        created() {
            var searchFilterQueryParam = this.$route.query.searchFilter;

            if (searchFilterQueryParam) {
                this.searchFilter = searchFilterQueryParam;
            }

            this.reloadData();
        },
        beforeRouteUpdate(to, from) {
            var searchFilterQueryParam = to.query.searchFilter;

            if (searchFilterQueryParam) {
                this.searchFilter = searchFilterQueryParam;
                history.replaceState(null, '', '/games/featured?searchFilter=' + searchFilterQueryParam);
            }
        }
    }
</script>

<style scoped>
    /* grid view */
    .game-grid-item {
        border-bottom: 0.5px solid #fff;
        margin: 0.2rem;
        padding: 0.5rem;
    }

    .game-grid-item img {
        width: 100%;
        height: auto;
        display: block;
        margin: 0 auto;
        text-align: center;
    }
    /* grid view */

    /* list view */
    .game-data-list-item {
        display: flex;
        justify-content: start;
        align-items: center;
        padding: 0 1rem;
        border-bottom: 1px solid #d9dad9;
    }

    .p-panel-content {
        padding: 1rem;
    }

    .game-data-list-item img {
        width: 250px;
        height: auto;
    }

    @media (max-width: 496px) {
        .game-data-list-item img {
            width: 300px; 
            height: auto;
            display: block;
            margin: 0 auto;
        }

        .game-data-list-item {
            display: block;
        }
    }
    /* list view */
</style>