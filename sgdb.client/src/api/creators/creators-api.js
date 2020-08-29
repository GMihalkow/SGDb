import creatorsBaseApi from './creators-base-api';

export default {
    getCurrentCreatorId() {
        return creatorsBaseApi.get('/Creators/GetCurrentCreatorId');
    },
    edit(data) {
        return creatorsBaseApi.patch('/Creators/Edit', data);
    }
};