<template>
    <div>
        <div v-if="validationContext.$dirty">
            <div v-bind:key="param ? param.id : Date.now()" v-for="param in validationContext.$flattenParams()">
                <div class="error" v-if="!validationContext[param.name]">{{validationMessage(param, propName, errorMessage)}}</div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'ValidationMessages',
        props: {
            propName: String,
            errorMessage: String,
            validationContext: Object
        },
        methods: {
            validationMessage: function (param, propName, errorMessage) {
                var params = param.params ? param.params : {};

                const validationMessages = {
                    required: '{0} is a required field.'.replace('{0}', propName),
                    minLength: '{0} must be at least {1} characters long.'.replace('{0}', propName).replace('{1}', params.min),
                    maxLength: '{0} must be maximum {1} characters long.'.replace('{0}', propName).replace('{1}', params.max),
                    email: '{0} must be a valid email format.'.replace('{0}', propName),
                    numeric: '{0} must contain only numeric values.'.replace('{0}', propName),
                    sameAs: '{0} must be the same as {1}.'.replace('{0}', propName).replace('{1}', params.eq),
                    url: '{0} must a valid url address.'.replace('{0}', propName),
                    decimal: '{0} must a valid decimal number.'.replace('{0}', propName),
                    minValue: '{0} has a minimum value of {1}.'.replace('{0}', propName).replace('{1}', params.min),
                    validImageUrl: '{0} must be a valid image url using HTTPS. Allowed image formats are jpeg, jpg, png & gif.'.replace('{0}', propName),
                    not: errorMessage
                };

                return validationMessages[param.name];
            }
        }
    }
</script>

<!-- TODO [GM]: remove? -->
<style scoped></style>