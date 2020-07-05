<template>
    <div v-if="errors.length > 0" class="p-field p-col-12 p-md-8 p-sm-12 text-center">
        <ul class="validation-summary" v-bind:key="error.id" v-for="error in errors">
            <li class="error">{{error}}</li>
        </ul>
    </div>
</template>

<script>
    export default {
        name: 'ValidationSummary',
        props: {
            errors: Array
        }
    }
</script>

<style scoped>
    .validation-summary {
        list-style: none;
        text-decoration: none;
    }
</style>