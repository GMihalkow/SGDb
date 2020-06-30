import baseApi from '../base-api';

var baseUrl = 'https://localhost:5005/api/Admin';

export default {
    creators: {
        getAll() {
            return baseApi.get(baseUrl + '/Creators/GetAll');
        },
        edit(data) {
            return baseApi.patch(baseUrl + '/Creators/Edit', data);
        }
    }
};