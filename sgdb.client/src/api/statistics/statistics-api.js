import baseApi from '../base-api';

var baseUrl = 'http://localhost:5007/api';

export default {
    get() {
        return baseApi.get(baseUrl + '/Statistics/Get');
    }
};