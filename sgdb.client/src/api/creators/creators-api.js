import creatorsBaseApi from './creators-base-api';

export default {
    getAll() { return creatorsBaseApi.get('/Creators/GetAll'); },
    // create(data) { return creatorsBaseApi.postWithFormData('/api/Creators/Create', data); }
};