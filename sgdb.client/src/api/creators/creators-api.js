import creatorsBaseApi from './creators-base-api';

export default {
    getCurrentCreatorId() {
        return creatorsBaseApi.get('/Creators/GetCurrentCreatorId');
    },
    getAll() {
        return creatorsBaseApi.get('/Creators/GetAll');
    },
    edit(data) {
        return creatorsBaseApi.patch('/Creators/Edit', data);
    }
};