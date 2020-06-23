import baseApi from '../base-api';

// TODO [GM]: Extract in environment file.
var baseUrl = 'https://localhost:5001';

export default {
    // TODO [GM]: Implement
    get(endpoint, queryString) { return baseApi.get(baseUrl + endpoint); }
};