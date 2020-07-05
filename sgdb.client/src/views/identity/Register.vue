<template>
    <form action="#" @submit.prevent="submit" class="w-mx-60r p-1 m1r-auto">
        <div class="p-grid p-formgrid py-1 p-fluid p-justify-center">
            <h2 class="p-col-12 p-md-8 p-sm-12 m1r-auto text-center">Register</h2>
            
            <ValidationSummary v-bind:errors="errors"/>
            
            <div class="p-field p-col-12 p-md-8 p-sm-12">
                <div class="p-inputgroup">
                    <span class="p-inputgroup-addon">
                        <i class="pi pi-user"></i>
                    </span>
                    <InputText 
                        v-model="form.username" 
                        v-bind:class="{ 'p-invalid': !$v.form.username.minLength || !$v.form.username.required }"
                        @blur="setFieldValue('username', $event.target.value)"
                        placeholder="Username" />
                </div>
                <ValidationMessages v-bind:validationContext="$v.form.username" v-bind:propName="'Username'" />
            </div>

            <div class="p-field p-col-12 p-md-8 p-sm-12">
                <div class="p-inputgroup">
                    <span class="p-inputgroup-addon">
                        <i class="pi pi-user"></i>
                    </span>
                    <InputText v-model="form.firstName" placeholder="First name" />
                </div>
            </div>

            <div class="p-field p-col-12 p-md-8 p-sm-12">
                <div class="p-inputgroup">
                    <span class="p-inputgroup-addon">
                        <i class="pi pi-user"></i>
                    </span>
                    <InputText v-model="form.lastName" placeholder="Last name" />
                </div>
            </div>

            <div class="p-field p-col-12 p-md-8 p-sm-12">
                <div class="p-inputgroup">
                    <span class="p-inputgroup-addon">
                        <i class="pi pi-inbox"></i>
                    </span>
                    <InputText
                        v-model="form.emailAddress" 
                        v-bind:class="{ 'p-invalid': !$v.form.emailAddress.minLength || !$v.form.emailAddress.required || !$v.form.emailAddress.email }"
                        @blur="setFieldValue('emailAddress', $event.target.value)"
                        placeholder="Email Address" />
                </div>
                <ValidationMessages v-bind:validationContext="$v.form.emailAddress" v-bind:propName="'Email Address'" />
            </div>

            <div class="p-field p-col-12 p-md-8 p-sm-12">
                <div class="p-inputgroup">
                    <span class="p-inputgroup-addon">
                        <i class="pi pi-mobile"></i>
                    </span>
                    <InputText 
                        v-model="form.phoneNumber" 
                        v-bind:class="{ 'p-invalid': !$v.form.phoneNumber.minLength || !$v.form.phoneNumber.maxLength
                            || !$v.form.phoneNumber.required || !$v.form.phoneNumber.numeric }"
                        @blur="setFieldValue('phoneNumber', $event.target.value)"
                        placeholder="Phone"
                     />
                </div>
                <ValidationMessages v-bind:validationContext="$v.form.phoneNumber" v-bind:propName="'Phone'" />
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
                        placeholder="Password"/>
                </div>
                <ValidationMessages v-bind:validationContext="$v.form.password" v-bind:propName="'Password'" />
            </div>

            <div class="p-field p-col-12 p-md-8 p-sm-12">
                <div class="p-inputgroup">
                    <span class="p-inputgroup-addon">
                        <i class="pi pi-lock"></i>
                    </span>
                    <Password 
                        v-model="form.confirmPassword" 
                        v-bind:class="{ 'p-invalid': !$v.form.confirmPassword.required || !$v.form.confirmPassword.sameAs 
                            || !$v.form.confirmPassword.minLength || !$v.form.confirmPassword.maxLength }"
                        @blur="setFieldValue('confirmPassword', $event.target.value)"
                        placeholder="Confirm password"/>
                </div>
                <ValidationMessages v-bind:validationContext="$v.form.confirmPassword" v-bind:propName="'Confirm Password'" />
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
    import creatorsApi from '../../api/creators/creators-api';
    
    import { required, minLength, email, numeric, maxLength, sameAs } from 'vuelidate/lib/validators';

    export default {
        name: 'Register',
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
                    username: '',
                    firstName: '',
                    lastName: '',
                    emailAddress: '',
                    phoneNumber: '',
                    password: '',
                    confirmPassword: ''
                },
                errors: []
            };
        },
        validations: {
            form: {
                username: {
                    required: required,
                    minLength: minLength(6),
                    maxLength: maxLength(50)
                },
                emailAddress: {
                    required: required,
                    minLength: minLength(6),
                    email: email
                },
                phoneNumber: {
                    required: required,
                    numeric: numeric,
                    minLength: minLength(10),
                    maxLength: maxLength(12)
                },
                password: {
                    required: required,
                    minLength: minLength(6),
                    maxLength: maxLength(50)
                },
                confirmPassword: {
                    required: required,
                    sameAs: sameAs('password'),
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
                    
                    // formObj.append('Username', this.form.username);
                    formObj.append('FirstName', this.form.firstName);
                    formObj.append('LastName', this.form.lastName);
                    formObj.append('Password', this.form.password);
                    formObj.append('ConfirmPassword', this.form.confirmPassword);
                    formObj.append('EmailAddress', this.form.emailAddress);
                    formObj.append('PhoneNumber', this.form.phoneNumber);

                    _this.$store.dispatch('authenticate', { endpoint: 'register', creds: formObj, successCallback: function() {
                        var creatorsFormObj = new FormData();
                        
                        creatorsFormObj.append('Username', _this.form.username);
                        
                        creatorsApi.create(creatorsFormObj).then(function(res){
                            console.log(res);
                        }).catch(function(err) {
                            console.log(err);
                        });
                        
                    }, 
                    successCallback: function(){
                        _this.$store.dispatch('setAuthHeader');
                    },
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