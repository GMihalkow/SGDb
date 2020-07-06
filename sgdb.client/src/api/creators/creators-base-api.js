import baseApi from '../base-api';

// TODO [GM]: Extract in environment file.
var baseUrl = 'https://localhost:5001/api';

export default {
    get(endpoint, queryParams) { return baseApi.get(baseUrl + endpoint, queryParams); },
    patch(endpoint, data) {  return baseApi.patch(baseUrl + endpoint, data); },
    postWithFormData(endpoint, data) { return baseApi.postWithFormData(baseUrl + endpoint, data); }
};