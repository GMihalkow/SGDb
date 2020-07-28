<template>
    <div class="nav-container main-bg-dark">
        <nav class="nav">
            <router-link to="/">Home</router-link>
            <router-link to="/games/featured">Games</router-link>

            <Menubar v-if="isLoggedIn" :model="adminMenuModel" />

            <div class="p-fluid d-flex nav-search-wrapper"> 
                <AutoComplete 
                    v-model="selectedGameName" 
                    @item-select="redirectToGameDetailsWithId" 
                    :suggestions="gameNames" 
                    @complete="searchGame($event)" 
                    placeholder="Search for games by name..." 
                    field="name" />
                <Button icon="pi pi-search" class="p-button-primary p-button" @click="redirectToFeaturedGamesWithAppliedFilter"/>
            </div>
            
            <router-link v-if="!isLoggedIn" to="/identity/login">Login</router-link>
            <router-link v-if="!isLoggedIn" to="/identity/register">Register</router-link>
            <a href="javascript:void(0)" v-if="isLoggedIn" v-on:click="logout()">Logout</a>
        </nav>
    </div>
</template>

<script>
    import AutoComplete from 'primevue/autocomplete';
    import Button from 'primevue/button';
    import Menubar from 'primevue/menubar';
    import gamesApi from '../../api/creators/games-api';
    import creatorsApi from '../../api/creators/creators-api';

    import { mapGetters } from 'vuex';
    import { roles } from '../../helpers/constants/roles';
    
    export default {
        name: 'Nav',
        data() {
            return {
                selectedGameName: null,
                gameNames: [],
                adminMenuModel: [
                {
                    label: 'Tools',
                    icon:'pi pi-fw pi-cog',
                    items: []
                }]
            }
        },
        components: {
            Button: Button,
            AutoComplete: AutoComplete,
            Menubar: Menubar
        },
        methods: {
            searchGame(event) {
                var _this = this;

                var result = gamesApi.getAllForAutoComplete().then(function(res) {
                    var games = res.data.data;
                
                    _this.gameNames = games.filter(function(g) { return g.name.toLowerCase().startsWith(event.query.toLowerCase()); });
                });
            },
            logout() {
                this.$store.dispatch('logout');
            },
            redirectToGameDetailsWithId(param) {
                if (this.$route.name === 'gameDetails') {
                    // catch is just for silencing the duplicates error
                    this.$router.replace({ name: 'gameDetails', params: { id: param.value.id } }).catch(function() {});
                } else {
                    this.$router.push({ name: 'gameDetails', params: { id: param.value.id } });
                }
            },
            redirectToFeaturedGamesWithAppliedFilter() {
                var _this = this;

                var searchValue;
                
                if (typeof _this.selectedGameName == 'object' && _this.selectedGameName != null ) {
                    searchValue = _this.selectedGameName.name;
                } else if(typeof _this.selectedGameName == 'string') {
                    searchValue = _this.selectedGameName;
                }

                if (this.$route.name === 'featuredGames') {
                    // catch is just for silencing the duplicates error
                    this.$router.replace({ name: 'featuredGames', query: { searchFilter: searchValue } }).catch(function() {});
                } else {
                    this.$router.push({ name: 'featuredGames', query: { searchFilter: searchValue } });
                }
            }
        },
        computed: {
            ...mapGetters({'isLoggedIn': 'isLoggedIn'}),
            ...mapGetters({'role': 'role'}),
            ...mapGetters({'isUserAdmin': 'isUserAdmin'}),
            ...mapGetters({'isUserCreator': 'isUserCreator'})
        },
        mounted() {
            //TODO [GM]: remove _this?
            var _this = this;

            var creatorsLinkObj = { label: 'Creators', visible: _this.isUserAdmin, to: '/admin/creators/search' };

            _this.adminMenuModel[0].items = [
                { label: '...', command:() => {} },
                { label: 'Views History', to: '/games/userViewedGamesHistory' },
                { label: 'Change Password', visible: _this.isLoggedIn, to: '/identity/changepassword' },
                { separator: true },
                { label: 'Creators', visible: _this.isUserAdmin, to: '/admin/creators/search' },
                { label: 'Games', visible: _this.isUserAdmin || _this.isUserCreator, to: '/admin/games/search' },
                { label: 'Publishers', visible: _this.isUserAdmin || _this.isUserCreator, to: '/admin/publishers/search' }
            ];
        },
        updated() {
            this.adminMenuModel[0].items = this.adminMenuModel[0].items || [];

            var separatorIndex = this.adminMenuModel[0].items.findIndex(function(menuObj) { return menuObj.separator; });

            var items = this.adminMenuModel[0].items.filter(function(menuObj, index) { return index <= separatorIndex; });

            items.push({ label: 'Creators', visible: this.isUserAdmin, to: '/admin/creators/search' });
            items.push({ label: 'Games', visible: this.isUserAdmin || this.isUserCreator, to: '/admin/games/search' });
            items.push({ label: 'Publishers', visible: this.isUserAdmin || this.isUserCreator, to: '/admin/publishers/search' });

            this.adminMenuModel[0].items = items;
        }
    };
</script>

<style scoped>
    .nav {
        display: flex;
        color: #fff;
        justify-content: center;
        flex-wrap: wrap;
        max-width: 60rem;
        margin: 0 auto;
    }

    .nav-container {
        width: 100%;
    }

    .p-button {
        margin: 0.6rem 0;
    }

    .p-autocomplete {
        display: block;
        margin: 0.6rem 0;
        min-width: 30rem;
    }

    .p-menubar {
        padding: 0;
        margin: 0.5rem;
    }

    .nav a {
        padding: 1rem 0.5rem;
        text-decoration: none;
        color: #fff;
        font-weight: bold;
        outline: none;
    }

    @media (max-width: 860px) { 
        .nav-search-wrapper {
            display: none;
        }
    }
</style>