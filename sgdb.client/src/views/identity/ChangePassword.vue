<template>
    <form action="#" @submit.prevent="submit" class="w-mx-60r p-1 m1r-auto">
        <div class="p-grid p-formgrid py-1 p-fluid p-justify-center">
            <h2 class="p-col-12 p-md-8 p-sm-12 m1r-auto text-center">Change Password</h2>
            
            <ValidationSummary v-bind:errors="errors"/>
            
            <div class="p-field p-col-12 p-md-8 p-sm-12">
                <div class="p-inputgroup">
                    <span class="p-inputgroup-addon">
                        <i class="pi pi-inbox"></i>
                    </span>
                    <Password 
                        v-model="form.currentPassword" 
                        v-bind:class="{ 'p-invalid': !$v.form.currentPassword.required }"
                        @blur="setFieldValue('currentPassword', $event.target.value)"
                        v-bind:feedback="false"
                        placeholder="Current password" />
                </div>
                <ValidationMessages v-bind:validationContext="$v.form.currentPassword" v-bind:propName="'Current password'" />
            </div>

            <div class="p-field p-col-12 p-md-8 p-sm-12">
                <div class="p-inputgroup">
                    <span class="p-inputgroup-addon">
                        <i class="pi pi-lock"></i>
                    </span>
                    <Password 
                        v-model="form.newPassword" 
                        v-bind:class="{ 'p-invalid': !$v.form.newPassword.required || !$v.form.newPassword.not }"
                        @blur="setFieldValue('newPassword', $event.target.value)"
                        placeholder="New password"/>
                </div>
                <ValidationMessages 
                    v-bind:propName="'New password'" 
                    v-bind:validationContext="$v.form.newPassword" 
                    v-bind:errorMessage="'The new password must be different from the current password.'" />
            </div>

            <div class="p-col-12 p-md-8 p-sm-12 ">
                <Button type="submit" class="p-button-secondary" label="Submit" />
            </div>
        </div>

        <Toast/>
    </form>
</template>

<script>
    import 'primeflex/primeflex.css';
    import Password from 'primevue/password';
    import Button from 'primevue/button';
    import Toast from 'primevue/toast';
    import ValidationSummary from '../../components/partials/ValidationSummary';
    import ValidationMessages from '../../components/partials/ValidationMessages';

    import { required, sameAs, not } from 'vuelidate/lib/validators';
    import identityApi from '../../api/identity/identity-api';

    export default {
        name: 'ChangePassword',
        components: {
            Password: Password,
            Button: Button,
            Toast: Toast,
            ValidationSummary: ValidationSummary,
            ValidationMessages: ValidationMessages
        },
        data() {
            return {
                form: {
                    currentPassword: '',
                    newPassword: ''
                },
                errors: []
            }
        },
        validations: {
            form: {
                currentPassword: {
                    required: required
                },
                newPassword: {
                    not: not(sameAs('currentPassword')),
                    required: required
                }
            }
        },
        methods: {
            // TODO [GM]: Extract to external component and maybe inherit it for every form?
            setFieldValue(fieldName, value) {
                this.form[fieldName] = value;
                this.$v.form[fieldName].$touch();
            },
            submit() {
                var _this = this;

                if (!_this.$v.$invalid) {
                    identityApi.changePassword(_this.form).then(function(res) {
                        _this.$store.dispatch('logout');
                    }).catch(function(err) {
                        if (err.response){
                            var data = err.response.data;
             
                            if (!data.succeeded){
                                _this.$toast.add({severity: 'error', summary: data.errors[0] ? data.errors[0] : 'Something went wrong.', life: 3000});
                            }
                        } else {
                            _this.$toast.add({severity: 'error', summary: 'Something went wrong.', life: 3000});
                        }
                    });
                }
            }
        }
    }
</script>

<style scoped></style>