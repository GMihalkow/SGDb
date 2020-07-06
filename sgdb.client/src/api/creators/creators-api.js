import creatorsBaseApi from './creators-base-api';

export default {
    getAll() {
        return creatorsBaseApi.get('/Creators/GetAll');
    },
    edit(data) {
        return creatorsBaseApi.patch('/Creators/Edit', data);
    }
};