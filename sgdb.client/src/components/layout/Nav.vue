<template>
    <div class="nav-container main-bg-dark">
        <nav class="nav">
            <Menubar :model="menuModel">
                <template #end>
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
                </template>
            </Menubar>
        </nav>
    </div>
</template>

<script>
    import AutoComplete from 'primevue/autocomplete';
    import Button from 'primevue/button';
    import Menubar from 'primevue/menubar';
    import gamesApi from '../../api/creators/games-api';

    import { mapGetters } from 'vuex';
    import { roles } from '../../helpers/constants/roles';
    
    export default {
        name: 'Nav',
        data() {
            return {
                selectedGameName: null,
                gameNames: [],
                menuModel: [
                    {
                        label: 'Home',
                        to: '/'
                    },
                    {
                        label: 'Games',
                        to: '/games/featured'
                    },
                    {
                        label: 'Tools',
                        icon:'pi pi-fw pi-cog',
                        items: [],
                        visible: this.$store.state.isLoggedIn
                    },
                    {
                        label: 'Login',
                        to: '/identity/login',
                        visible: !this.$store.state.isLoggedIn
                    },
                    {
                        label: 'Register',
                        to: '/identity/register',
                        visible: !this.$store.state.isLoggedIn
                    },
                    {
                        label: 'Logout',
                        visible: this.$store.state.isLoggedIn,
                        command: () => this.$store.dispatch('logout')
                    }
                ]
            }
        },
        components: {
            Button: Button,
            AutoComplete: AutoComplete,
            Menubar: Menubar
        },
        watch: {
            isLoggedIn() {
                this.$forceUpdate();
            }
        },
        methods: {
            searchGame(event) {
                var _this = this;

                gamesApi.getAllForAutoComplete().then(function(res) {
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
                var searchValue;
                
                if (typeof this.selectedGameName == 'object' && this.selectedGameName != null) {
                    searchValue = this.selectedGameName.name;
                } else if (typeof this.selectedGameName == 'string') {
                    searchValue = this.selectedGameName;
                }

                if (this.$route.name === 'featuredGames') {
                    // catch is just for silencing the duplicates error
                    this.$router.replace({ name: 'featuredGames', query: { searchFilter: searchValue } }).catch(function() {});
                } else {
                    this.$router.push({ name: 'featuredGames', query: { searchFilter: searchValue } });
                }
            },
            refreshToolsMenu() {
                if (this.isLoggedIn) {
                    this.toolsMenuBar.items = [
                        { label: '...' },
                        { label: 'Views History', to: '/games/userViewedGamesHistory' },
                        { label: 'Change Password', visible: this.isLoggedIn, to: '/identity/changepassword' },
                        { separator: true },
                        { label: 'Creators', visible: this.isUserAdmin, to: '/admin/creators/search' },
                        { label: 'Games', visible: this.isUserAdmin || this.isUserCreator, to: '/admin/games/search' },
                        { label: 'Publishers', visible: this.isUserAdmin || this.isUserCreator, to: '/admin/publishers/search' }
                    ];
                }
            }
        },
        computed: {
            toolsMenuBar() {
                return this.menuModel.find(function(item) { return item.label === 'Tools'; });
            },
            ...mapGetters({'isLoggedIn': 'isLoggedIn'}),
            ...mapGetters({'role': 'role'}),
            ...mapGetters({'isUserAdmin': 'isUserAdmin'}),
            ...mapGetters({'isUserCreator': 'isUserCreator'})
        },
        mounted() {
            this.refreshToolsMenu();
        },
        updated() {
            this.toolsMenuBar.visible = this.isLoggedIn;

            this.refreshToolsMenu();

            const _this = this;

            this.menuModel.filter(function(mModel) {
                return ['Register', 'Login', 'Logout'].indexOf(mModel.label) > -1; 
            }).forEach(function(mModel) {
                if (mModel.label === 'Logout') {
                    mModel.visible = _this.isLoggedIn;
                } else {
                    mModel.visible = !_this.isLoggedIn;
                }
            });
        }
    };
</script>

<style scoped>
    .nav {
        color: #fff;
        max-width: 60rem;
        margin: 0 auto;
    }

    .nav-container {
        width: 100%;
    }

    .p-button {
        margin: 0.6rem 0;
    }

    .nav-search-wrapper {
        margin: 0 0.5rem;
    }

    .p-autocomplete {
        display: block;
        margin: 0.6rem 0;
        min-width: 15rem;
    }

    .p-menubar {
        padding: 0;
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