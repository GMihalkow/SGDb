import baseApi from '../base-api';

var baseUrl = 'http://localhost:5003';

export default {
    getAll() {
        return baseApi.get(baseUrl + '/api/Roles/GetAll'); 
    },
    create(data) { 
        return baseApi.postWithFormData(baseUrl + '/api/Roles/Create', data); 
    },
    edit(data) { 
        return baseApi.patch(baseUrl + '/api/Roles/Edit', data); 
    },
    delete(data) { 
        return baseApi.delete(baseUrl + '/api/Roles/Delete', data); 
    }
}