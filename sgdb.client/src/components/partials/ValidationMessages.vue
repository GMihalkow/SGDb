<template>
    <div>
        <div v-if="validationContext.$dirty">
            <div v-bind:key="param.id" v-for="param in validationContext.$params">
                <div class="error" v-if="!validationContext[param.type]">{{validationMessage(param, propName)}}</div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'ValidationMessages',
        props: {
            propName: String,
            validationContext: Object
        },
        methods: {
            validationMessage: function (prop, propName) {
                const validationMessages = {
                    required: '{0} is a required field.'.replace('{0}', propName),
                    minLength: '{0} must be at least {1} characters long.'.replace('{0}', propName).replace('{1}', prop.min),
                    maxLength: '{0} must be maximum {1} characters long.'.replace('{0}', propName).replace('{1}', prop.max),
                    email: '{0} must be a valid email format.'.replace('{0}', propName),
                    numeric: '{0} must contain only numeric values.'.replace('{0}', propName),
                    sameAs: '{0} must be the same as {1}.'.replace('{0}', propName).replace('{1}', prop.eq)
                };

                return validationMessages[prop.type];
            }
        }
    }
</script>

<!-- TODO [GM]: remove? -->
<style scoped>

</style>