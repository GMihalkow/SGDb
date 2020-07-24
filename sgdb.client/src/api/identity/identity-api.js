import baseApi from './../base-api';

// TODO [GM]: Extract in environment file.
var baseUrl = 'http://localhost:5003';

export default {
    register(data) { return baseApi.postWithFormData(baseUrl + '/api/identity/register', data); },
    login(data) { return baseApi.postWithFormData(baseUrl + '/api/identity/login', data); },
    changePassword(data) { return baseApi.patch(baseUrl + '/api/Identity/ChangePassword', data); }
};