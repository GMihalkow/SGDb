<template>
    <div>
        <div v-show="!loading" class="game-details-bg-img-wrapper" v-bind:style="{ backgroundImage: 'url(' + game.backgroundImageUrl + ')'}">
            <div class="game-details-wrapper w-mx-60r mx-auto p-1">
                <div>
                    <h2 class="py-1">{{game.name}}</h2>
                </div>
                <div>
                    <div>
                        <img :src="game.headerImageUrl" :alt="game.name"/>
                        <ul>
                            <li>Recommendations: {{game.recommendations ? game.recommendations : '-'}}</li>
                            <li>Price: {{game.price ? game.price + '$' : '-'}}</li>
                            <li>Release Date: {{game.releasedOn ? new Date(game.releasedOn).toLocaleDateString() : 'TBD'}}</li>
                            <li>Views: {{game.views}}</li>
                            <li>Website: <a :href="game.websiteUrl">{{game.websiteUrl}}</a></li>
                        </ul>
                    </div>
                    <div>
                        <div v-html="game.about"></div>
                    </div>
                </div>
            </div>
        </div>
        <Toast />
        <ProgressSpinner v-show="loading"/>
    </div>
</template>

<script>
    import ProgressSpinner from 'primevue/progressspinner';
    import Toast from 'primevue/toast';
    import creatorsApiGateway from '../../api/creators-gateway/creators-api-gateway';

    export default {
        name: 'GameDetails',
        components: {
            ProgressSpinner: ProgressSpinner,
            Toast: Toast
        },
        data() {
            return {
                loading: false,
                game: {}
            };
        },
        methods: {
            loadData(gameId) {
                var _this = this;
                _this.loading = true;

                _this.game = {};
                
                creatorsApiGateway.getGameDetails(gameId).then(function(res){ 
                        var gameDetails = res.data.data;
                        
                        _this.loading = false;
                        _this.game = gameDetails;
                    })
                    .catch(function(err) {
                        if (err.response) {
                            var errors = err.response.data.errors;
                            _this.$toast.add({severity: 'error', summary: errors[0] ? errors[0] : 'Something went wrong.'});
                        } else {
                            _this.$toast.add({severity: 'error', summary: 'Something went wrong.'});
                        }
                    });
            }
        },
        created() {
            this.loadData(this.$route.params.id);
        },
        beforeRouteUpdate(to, from) {
            var gameId = to.params.id;

            this.loadData(gameId);
            history.replaceState(null, '', '/games/details/' + gameId);
        }
    }
</script>

<style scoped>
    .game-details-bg-img-wrapper {
        position: relative;
        background-repeat: no-repeat;
        background-position-x: center;
        color: #fff;
    }

    .p-progress-spinner {
        display: block;
        position: relative;
        margin: 0 auto;
    }

    .game-details-wrapper a {
        color: #fff !important;
    }

    .game-details-wrapper >>> ul {
        list-style: none;
    }

    .game-details-wrapper >>> img {
            max-width: 100%;
    }
</style>