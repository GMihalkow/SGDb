import baseApi from './base-api';

// TODO [GM]: Extract in environment file.
var baseUrl = 'https://localhost:5003';

export default {
    register(data) { return baseApi.postWithFormData(baseUrl + '/api/identity/register', data); },
    login(data) { return baseApi.postWithFormData(baseUrl + '/api/identity/login', data); }
};