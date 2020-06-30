<template>
    <div>
        <div class="bg-img-wrapper"></div>
        <div class="w-mx-60r m1r-auto">
            <h2 class="text-center mb-1">Featured Games</h2>
            <div class="p-grid p-justify-center">
                <div class="p-col-10 p-md-4 p-sm-8" v-bind:key="gameCard.id" v-for="gameCard in gameCards">
                    <Card>
                        <template slot="header">
                            <img alt="user header" v-bind:src="gameCard.headerUrl">
                        </template>
                        <template slot="title">{{gameCard.name}}</template>
                        <template slot="footer">
                            <Button icon="pi pi-check-square" label="Details" />
                        </template>
                    </Card>
                </div>
            </div>
            <h2 class="text-center mb-1">Statistics</h2>
            <div class="text-center mb-1">
                <h4>Total Games: {{statistics.totalGamesCount}}</h4>
                <h4>Total Genres: {{statistics.totalGenresCount}}</h4>
                <h4>Total Publishers: {{statistics.totalPublishersCount}}</h4> 
            </div>
        </div>
    </div>
</template>

<script>
    import Card from 'primevue/card';
    import Button from 'primevue/button';
    import gamesApi from '../api/creators/games-api';
    import statisticsApi from '../api/statistics/statistics-api';
    
    export default {
        name: 'Home',
        components: {
            Card: Card,
            Button: Button
        },
        data() {
            return {
                images: [],
                gameCards: [],
                statistics: {}
            }
        },
        mounted() {
            var _this = this;

            gamesApi.getGameIndexCards().then(function(res) {
                var gameCards = res.data.data;
                
                _this.gameCards = gameCards;
            }).catch(function(err) {
                console.log(err);
            });

            statisticsApi.get().then(function(res) {
                var statistics = res.data.data;
                
                _this.statistics = statistics;
            }).catch(function(err){
                console.log(err);
            });
        }
    }
</script>

<style scoped>
    .bg-img-wrapper {
        height: 44rem;
        background-repeat: no-repeat;
        background-size: cover;
        background-position: center;
        overflow: hidden;
        background-image: url('/img/bg.jpg');
        color: #fff;
    }
</style>