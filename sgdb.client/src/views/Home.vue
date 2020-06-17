<template>
    <div class="w-60r m1r-auto">
        <Galleria :value="images" :numVisible="5" :showThumbnails="false" style="max-width: 800px;" :circular="true" 
        :autoPlay="true" :transitionInterval="5000">
            <template #item="slotProps">
                <img :src="slotProps.item.headerImageUrl" style="width: 100%; display: block;" />
            </template>
             <template #caption="{item}">
                <h4 style="margin-bottom: .5rem;">{{item.name}}</h4>
                <p>{{item.about.substr(0, 50)}}...</p>
            </template>
        </Galleria>
    </div>
</template>

<script>
    import Galleria from 'primevue/galleria';
    // import baseApi from '../api/base-api.js';

    // Vue.component('Galleria', Galleria);

    export default {
        name: 'Home',
        components: {
            Galleria: Galleria
        },
        data() {
            return {
                images: []
            }
        },
        mounted() {
            // var _this = this;

            // baseApi.getAll().then(function(res) {
            //     console.log(res);

            //     _this.images = res.data.filter(function(d) { return d.headerImageUrl; });

            //     console.log(_this.images);
            // });
        }
    }
</script>

<style scoped>

</style>