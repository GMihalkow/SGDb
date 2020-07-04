<template>
    <form action="#" @submit.prevent="submit" class="w-mx-60r p-1 m1r-auto">
        <div class="p-grid p-formgrid py-1 p-fluid p-justify-center">
            <h2 class="p-col-12 p-md-8 p-sm-12 m1r-auto text-center">Login</h2>
            
            <ValidationSummary v-bind:errors="errors"/>
            
            <div class="p-field p-col-12 p-md-8 p-sm-12">
                <div class="p-inputgroup">
                    <span class="p-inputgroup-addon">
                        <i class="pi pi-inbox"></i>
                    </span>
                    <InputText 
                        v-model="form.email" 
                        v-bind:class="{ 'p-invalid': !$v.form.email.minLength || !$v.form.email.required }"
                        @blur="setFieldValue('email', $event.target.value)"
                        placeholder="Email" />
                </div>
                <ValidationMessages v-bind:validationContext="$v.form.email" v-bind:propName="'Email Address'" />
            </div>

            <div class="p-field p-col-12 p-md-8 p-sm-12">
                <div class="p-inputgroup">
                    <span class="p-inputgroup-addon">
                        <i class="pi pi-lock"></i>
                    </span>
                    <Password 
                        v-model="form.password" 
                        v-bind:class="{ 'p-invalid': !$v.form.password.required || !$v.form.password.minLength || !$v.form.password.maxLength }"
                        @blur="setFieldValue('password', $event.target.value)"
                        v-bind:feedback="false"
                        placeholder="Password"/>
                </div>
                <ValidationMessages v-bind:validationContext="$v.form.password" v-bind:propName="'Password'" />
            </div>

            <div class="p-col-12 p-md-8 p-sm-12 ">
                <Button type="submit" class="p-button-secondary" label="Submit" />
            </div>
        </div>
    </form>
</template>

<script>
    import 'primeflex/primeflex.css';
    import InputText from 'primevue/inputtext';
    import Password from 'primevue/password';
    import Button from 'primevue/button';
    import ValidationSummary from '../../components/partials/ValidationSummary';
    import ValidationMessages from '../../components/partials/ValidationMessages';

    import { required, minLength, email, maxLength } from 'vuelidate/lib/validators';

    export default {
        name: 'Login',
        components: {
            ValidationMessages: ValidationMessages,
            ValidationSummary: ValidationSummary,
            InputText: InputText,
            Password: Password,
            Button: Button
        },
        data () {
            return {
                form: {
                    email: '',
                    password: ''
                },
                errors: []
            };
        },
        validations: {
            form: {
                email: {
                    required: required,
                    email: email,
                    minLength: minLength(6),
                    maxLength: maxLength(70)
                },
                password: {
                    required: required,
                    minLength: minLength(6),
                    maxLength: maxLength(50)
                }
            }
        },
        methods: {
            setFieldValue(fieldName, value) {
                this.form[fieldName] = value;
                this.$v.form[fieldName].$touch();
            },
            submit() {
                var _this = this;

                if (!_this.$v.$invalid) {
                    var formObj = new FormData();
                    
                    formObj.append('EmailAddress', this.form.email);
                    formObj.append('Password', this.form.password);

                    _this.$store.dispatch('authenticate', { endpoint: 'login',
                        creds: formObj, 
                        errorCallback: function(error) {
                            if(error.response){
                                var data = error.response.data;

                                if (!data.succeeded) {
                                    _this.errors = data.errors;
                                }
                            } else {
                                _this.errors = ['Something went wrong.'];
                            }
                        }
                    });
                }
            }
        }
    }
</script>

<style scoped>
    
</style>