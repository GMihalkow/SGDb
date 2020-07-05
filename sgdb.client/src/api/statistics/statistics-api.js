import baseApi from '../base-api';

var baseUrl = 'https://localhost:5007/api';

export default {
    get() {
        return baseApi.get(baseUrl + '/Statistics/Get');
    }
};