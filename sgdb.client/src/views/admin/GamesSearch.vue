<template>
    <div class="w-mx-60r p-1 mx-auto">
        <div class="p-card">
            <div class="p-card-body">
                <DataTable :value="games" 
                        dataKey="id"
                        class="p-datatable-responsive"
                        columnResizeMode="fit"
                        :filters="filters"
                        :paginator="true" 
                        :rows="10"
                        :loading="loading"
                        :rowsPerPageOptions="[5,10,25,50]">
                    <template #header>
                        <div class="p-grid p-nogutter">
                            <div class="p-lg-10 p-sm-10 p-xs-12 text-start p-0">
                                List of Games
                                <span class="p-input-icon-left">
                                    <i class="pi pi-search" />
                                    <InputText v-model="filters['global']" placeholder="Search by name" />
                                </span>
                            </div>
                            <div class="p-lg-2 p-sm-2 p-xs-12 text-start p-0">
                                <div class="ml-auto w-min-content">
                                    <Button label="Create" class="p-button-primary" @click="openCreateGameDialog"/>
                                </div>
                            </div>
                        </div>
                    </template>
                     <template #empty>
                        No games found.
                    </template>
                    <template #loading>
                        Loading games data. Please wait.
                    </template>
                    <Column class="text-center" field="name" header="Name" :sortable="true" filterMatchMode="startsWith"/>
                    <Column class="text-center" field="price" header="Price" :sortable="true">
                        <template #body="slotProps">
                            <div>
                                {{slotProps.data.price}}$
                            </div>
                        </template>
                    </Column>
                    <Column class="text-center" field="views" header="Views" :sortable="true"/>
                    <Column class="text-center" field="recommendations" header="Recommendations" :sortable="true">
                        <template #body="slotProps">
                            <div>{{slotProps.data.recommendations ? slotProps.data.recommendations : '-'}}</div>
                        </template>
                    </Column>
                    <Column class="text-center" field="releasedOn" header="Release Date" :sortable="true">
                        <template #body="slotProps">
                            <div>{{new Date(slotProps.data.releasedOn).toLocaleDateString()}}</div>
                        </template>
                    </Column>
                    <Column header="Actions" headerStyle="text-align: center" bodyStyle="text-align: center; overflow: visible">
                        <template #body="slotProps">
                            <div v-if="currentCreatorId === slotProps.data.creatorId || isUserAdmin">
                                <Button type="button" icon="pi pi-cog" class="p-button-secondary p-mr-2" @click="openEditGameDialog(slotProps.data)"/>
                                <Button type="button" icon="pi pi-trash" class="p-button-danger" @click="openDeleteGameDialog(slotProps.data)"/>
                            </div>
                        </template>
                    </Column>
                </DataTable>

                <Toast />

                <Dialog 
                    class="p-fluid w-mx-60r overflow-y-md-auto overflow-y-sm-auto" 
                    :header="isEdit ? 'Edit Game' : 'New Game'"
                    :visible.sync="createGameDialog" 
                    :modal="true" 
                    :contentStyle="'overflow-y: visible;'">
                    <div class="p-grid p-nogutter p-justify-around">
                        <ValidationSummary v-bind:errors="errors"/>
                        
                        <div class="d-none">
                            <InputText type="hidden" v-model="form.id" />
                        </div>

                        <div class="p-field p-lg-5 p-md-5 p-col-12">
                            <label for="name">Name</label>
                            <InputText 
                                id="name" 
                                v-model.trim="form.name"
                                v-bind:class="{ 'p-invalid': !$v.form.name.required || !$v.form.name.maxLength }"
                                @blur="setFieldValue('name', $event.target.value)"
                                placeholder="Name" />
                            <ValidationMessages v-bind:validationContext="$v.form.name" v-bind:propName="'Name'" />
                        </div>
                        <div class="p-field p-lg-5 p-md-5 p-col-12">
                            <label for="headerImageUrl">Header Image Url</label>
                            <InputText id="headerImageUrl" 
                                v-model.trim="form.headerImageUrl"
                                v-bind:class="{ 'p-invalid': !$v.form.headerImageUrl.required || !$v.form.headerImageUrl.validImageUrl }"
                                @blur="setFieldValue('headerImageUrl', $event.target.value)"
                                placeholder="Header Image Url" />
                            <ValidationMessages v-bind:validationContext="$v.form.headerImageUrl" v-bind:propName="'Header Image Url'" />
                        </div>
                        <div class="p-field p-lg-11 p-md-11 p-col-12">
                            <label for="about">About</label>
                            <Textarea 
                                id="about" 
                                v-model.trim="form.about"
                                placeholder="About"
                                :autoResize="true" 
                                rows="3" 
                                cols="20" />
                        </div>
                        <div class="p-field p-lg-5 p-md-5 p-col-12">
                            <label for="backgroundImageUrl">Background Image Url</label>
                            <InputText id="backgroundImageUrl" 
                                v-model.trim="form.backgroundImageUrl"
                                v-bind:class="{ 'p-invalid': !$v.form.backgroundImageUrl.required || !$v.form.backgroundImageUrl.validImageUrl }"
                                @blur="setFieldValue('backgroundImageUrl', $event.target.value)"
                                placeholder="Background Image Url" />
                            <ValidationMessages v-bind:validationContext="$v.form.backgroundImageUrl" v-bind:propName="'Background Image Url'" />
                        </div>
                        <div class="p-field p-lg-5 p-md-5 p-col-12">
                            <label for="websiteUrl">Website Url</label>
                            <InputText id="websiteUrl" 
                                v-model.trim="form.websiteUrl"
                                v-bind:class="{ 'p-invalid': !$v.form.websiteUrl.url }"
                                @blur="setFieldValue('websiteUrl', $event.target.value)"
                                placeholder="Website Url" />
                            <ValidationMessages v-bind:validationContext="$v.form.websiteUrl" v-bind:propName="'Website Url'" />
                        </div>
                        <div class="p-field p-lg-5 p-md-5 p-col-12">
                            <label for="price">Price</label>
                            <InputText id="price" 
                                v-model.number="form.price" 
                                v-bind:class="{ 'p-invalid': !$v.form.price.decimal || !$v.form.price.minValue }"
                                @blur="setFieldValue('price', $event.target.value)"
                                placeholder="Price" />
                            <ValidationMessages v-bind:validationContext="$v.form.price" v-bind:propName="'Price'" />
                        </div>
                        <div class="p-field p-lg-5 p-md-5 p-col-12">
                            <label for="releasedOn">Released On</label>
                            <Calendar 
                                id="icon" 
                                :type="'date'"
                                :showIcon="true" 
                                v-model="form.releasedOn" 
                                placeholder="Released On" 
                                :monthNavigator="true" 
                                :yearNavigator="true" 
                                yearRange="1980:2020" />
                        </div>
                        <div class="p-field p-lg-5 p-md-5 p-col-12">
                            <label for="publishers">Publishers</label>
                            <MultiSelect 
                                id="publishers" 
                                v-model="selectedPublishers" 
                                :options="publishers" 
                                :filter="true"
                                optionLabel="name" 
                                placeholder="Select Publishers" />
                        </div>
                        <div class="p-field p-lg-5 p-md-5 p-col-12">
                            <label for="genres">Genres</label>
                            <MultiSelect 
                                id="genres" 
                                v-model="selectedGenres" 
                                :options="genres" 
                                :filter="true"
                                optionLabel="name" 
                                placeholder="Select Genres" />
                        </div>
                    </div>
                    <template #footer>
                        <Button v-if="!isEdit" label="Create" icon="pi pi-check" class="p-button-success p-button-text" @click="confirmCreateGame" />
                        <Button v-else label="Update" icon="pi pi-check" class="p-button-success p-button-text" @click="confirmEditGame" />
                        <Button label="Cancel" icon="pi pi-times" class="p-button-text" @click="closeDialogs"/>
                    </template>
                </Dialog>

                <Dialog header="Delete Game" :visible.sync="deleteGameDialog" :style="{width: '350px'}" :modal="true">
                    <div class="confirmation-content">
                        <i class="pi pi-exclamation-triangle p-mr-3" style="font-size: 2rem" />
                        <span>Are you sure you want to delete this game?</span>
                    </div>
                    <template #footer>
                        <Button label="Yes" icon="pi pi-check" @click="confirmDeleteGame" class="p-button-success p-button-text" autofocus />
                        <Button label="No" icon="pi pi-times" @click="closeDialogs" class="p-button-text" />
                    </template>
                </Dialog>
            </div>
        </div>
    </div>
</template>

<script>
    // import 'primeflex/primeflex.css';
    import DataTable from 'primevue/datatable';
    import Column from 'primevue/column';
    import InputText from 'primevue/inputtext';
    import Textarea from 'primevue/textarea';
    import Calendar from 'primevue/calendar';
    import Button from 'primevue/button';
    import MultiSelect from 'primevue/multiselect';
    import Dialog from 'primevue/dialog';
    import Toast from 'primevue/toast';
    import ValidationSummary from '../../components/partials/ValidationSummary';
    import ValidationMessages from '../../components/partials/ValidationMessages';
    import gamesApi from '../../api/creators/games-api';
    import creatorsApiGateway from '../../api/creators-gateway/creators-api-gateway';
    import publishersApi from '../../api/creators/publishers-api';
    import genresApi from '../../api/creators/genres-api';
    
    import { required, url, maxLength, decimal, minValue, between } from 'vuelidate/lib/validators';
    import { mapGetters } from 'vuex';
    
    const validImageUrl = function(param) { 
        var regExp = new RegExp(/^(https?:)?\/\/?[^'"<>]+?\.(jpg|jpeg|gif|png)(|\?[^'"<>]*)$/);

        return regExp.test(param);
    };

    export default {
        name: 'GamesSearch',
        data() {
            return {
                games: [],
                game: {},
                form: {
                    id: 0,
                    name: ''  ,
                    about: '',
                    headerImageUrl: '',
                    backgroundImageUrl: '',
                    websiteUrl: '',
                    price: 0,
                    releasedOn: undefined
                },
                publishers: [],
                selectedPublishers: [],
                genres: [],
                selectedGenres:[],
                errors: [],
                creator: {},
                filters: {},
                deleteGameDialog: false,
                createGameDialog: false,
                isEdit: false,
                loading: false
            };
        },
        computed: {
            ...mapGetters({'currentCreatorId': 'creatorId'}),
            ...mapGetters({'isUserAdmin': 'isUserAdmin'})
        },
        components: {
            DataTable: DataTable,
            Column: Column,
            InputText: InputText,
            Textarea: Textarea,
            Calendar: Calendar,
            MultiSelect: MultiSelect,
            ValidationMessages: ValidationMessages,
            ValidationSummary: ValidationSummary,
            Dialog: Dialog,
            Toast: Toast,
            Button: Button
        },
        validations: {
            form: {
                name: {
                    required: required,
                    maxLength: maxLength(500) 
                },
                headerImageUrl: {
                    required: required,
                    validImageUrl: validImageUrl
                },
                backgroundImageUrl: {
                    required: required,
                    validImageUrl: validImageUrl
                },
                websiteUrl: {
                    url: url
                },
                price: {
                    decimal: decimal,
                    minValue: minValue(0)
                }
            }
        },
        methods: {
            setFieldValue(fieldName, value) {
                this.form[fieldName] = value;
                this.$v.form[fieldName].$touch();
            },
            openDeleteGameDialog(game) {
                this.game = game;
                this.deleteGameDialog = true;   
            },
            openCreateGameDialog(game) {
                this.isEdit = false;
                
                this.form = {
                    name: ''  ,
                    about: '',
                    headerImageUrl: '',
                    backgroundImageUrl: '',
                    websiteUrl: '',
                    price: 0,
                    releasedOn: undefined
                };

                this.selectedGenres = [];
                this.selectedPublishers = [];

                this.createGameDialog = true;   
            },
            openEditGameDialog(game) {
                this.isEdit = true;

                var newFormObj = game;
                newFormObj.releasedOn = this.form.releasedOn ? new Date(this.form.releasedOn) : '';

                Object.assign(this.game, game);
                Object.assign(this.form, newFormObj);

                this.createGameDialog = true;   

                this.selectedPublishers = game.publisherIds ? this.publishers.filter(function(p) { return game.publisherIds.indexOf(p.id) >= 0; }) : [];
                this.selectedGenres = game.genreIds ? this.genres.filter(function(gnr) { return game.genreIds.indexOf(gnr.id) >= 0; }) : [];
            },
            confirmCreateGame() {
                var _this = this;

                if (!_this.$v.$invalid) {
                    var formObj = new FormData();
                    
                    formObj.append('Name', _this.form.name);
                    formObj.append('About', _this.form.about);
                    formObj.append('HeaderImageUrl', _this.form.headerImageUrl);
                    formObj.append('BackgroundImageUrl', _this.form.backgroundImageUrl);
                    formObj.append('WebsiteUrl', _this.form.websiteUrl);
                    formObj.append('Price', _this.form.price);

                    if (_this.form.releasedOn) {
                        formObj.append('ReleasedOn', _this.form.releasedOn.toISOString().substring(0, 10));
                    }

                    if (_this.selectedPublishers.length) {
                        _this.selectedPublishers.forEach(function(sp) {
                            formObj.append('PublisherIds', sp.id);
                        });
                    }

                    if (_this.selectedGenres.length) {
                        _this.selectedGenres.forEach(function(sg) {
                            formObj.append('GenreIds', sg.id);
                        });
                    }

                    gamesApi.create(formObj).then(function() {
                        _this.closeDialogs();

                        _this.$toast.add({severity: 'success', summary: 'Game Created.', life: 3000});

                        _this.reloadData();
                    }).catch(function(err) {
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
            },
            confirmEditGame() {
                // TODO [GM]: Consolidate with Create and reuse code?
                var _this = this;
    
                if (!_this.$v.$invalid) {
                    var formObj = new FormData();
                    
                    formObj.append('Id', _this.form.id);
                    formObj.append('Name', _this.form.name);
                    formObj.append('About', _this.form.about);
                    formObj.append('HeaderImageUrl', _this.form.headerImageUrl);
                    formObj.append('BackgroundImageUrl', _this.form.backgroundImageUrl);
                    formObj.append('WebsiteUrl', _this.form.websiteUrl);
                    formObj.append('Price', _this.form.price);

                    if (_this.form.releasedOn) {
                        formObj.append('ReleasedOn', _this.form.releasedOn.toISOString().substring(0, 10));
                    }

                    if (_this.selectedPublishers.length) {
                        _this.selectedPublishers.forEach(function(sp) {
                            formObj.append('PublisherIds', sp.id);
                        });
                    }

                    if (_this.selectedGenres.length) {
                        _this.selectedGenres.forEach(function(sg) {
                            formObj.append('GenreIds', sg.id);
                        });
                    }

                    gamesApi.edit(formObj).then(function() {
                        _this.closeDialogs();

                        var newVal = {};

                        Object.assign(newVal, _this.form);
                        
                        newVal.publisherIds = _this.selectedPublishers.map(function(sp) { return sp.id; });
                        newVal.genreIds = _this.selectedGenres.map(function(sg) { return sg.id; });

                        _this.reloadData();

                        _this.$toast.add({severity: 'success', summary: 'Game Updated Successfully.', life: 3000});

                        _this.game = {};
                    }).catch(function(err) {
                        _this.$set(_this.games, _this.games.findIndex(function(g) { return g.id === _this.game.id; }), _this.game);

                        _this.game = {};
                        _this.closeDialogs();
                        _this.$toast.add({severity: 'error', summary: 'Something went wrong.', life: 3000});

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
            },
            confirmDeleteGame() {
                var _this = this;
                
                gamesApi.delete({id: _this.game.id}).then(function() {
                    _this.reloadData();
                    _this.deleteGameDialog = false;
                    _this.game = {};

                    _this.$toast.add({severity: 'success', summary: 'Game Deleted.', life: 3000});
                }).catch(function() {
                    _this.$toast.add({severity: 'error', summary: 'Something went wrong.', life: 3000});

                    _this.deleteGameDialog = false;
                    _this.game = {};
                });

                _this.closeDialogs();
            },
            closeDialogs() {
                this.deleteGameDialog = false;
                this.createGameDialog = false;
            },
            reloadData() {
                var _this = this;
                _this.loading = true;
                _this.games = [];

                var loadedRecords = {
                    games: false,
                    publishers: false,
                    genres: false
                };

                creatorsApiGateway.getAllSearchGames().then(function(res) {
                    var games = res.data.data;
                    
                    _this.games = games;

                    loadedRecords.games = true;
                    if (loadedRecords.genres && loadedRecords.publishers) {
                        _this.loading = false;
                    }
                }).catch(function(err) {
                    loadedRecords.games = true;
                    if (loadedRecords.genres && loadedRecords.publishers) {
                        _this.loading = false;
                    }

                    _this.$toast.add({severity: 'error', summary: 'Something went wrong.', life: 3000});
                });

                publishersApi.getAllPublishersForMultiselect().then(function(res) {
                    var publishers = res.data.data;

                    _this.publishers = publishers;

                    loadedRecords.publishers = true;
                    if (loadedRecords.games && loadedRecords.genres) {
                        _this.loading = false;
                    }
                }).catch(function(err) {
                    loadedRecords.publishers = true;
                    if (loadedRecords.games && loadedRecords.genres) {
                        _this.loading = false;
                    }

                    console.log(err);
                });

                genresApi.getAllGenresForMultiselect().then(function(res) {
                    var genres = res.data.data;

                    _this.genres = genres;

                    loadedRecords.genres = true;
                    if (loadedRecords.publishers && loadedRecords.games) {
                        _this.loading = false;
                    }
                }).catch(function(err) {
                    loadedRecords.genres = true;
                    if (loadedRecords.publishers && loadedRecords.games) {
                        _this.loading = false;
                    }

                    console.log(err);
                });
            }
        },
        created() {
            this.reloadData();
        }
    }
</script>

<style scoped>
    .p-inputtextarea {
        height: 150px !important;
        overflow-y: auto !important;
    }
</style>