import creatorsBaseApi from './creators-base-api';

export default {
    getAllFeatured() { return creatorsBaseApi.get('/Games/GetAllFeatured'); },
    getGameIndexCards() { return creatorsBaseApi.get('/Games/GetGameIndexCards'); },
    getAllForAutoComplete() { return creatorsBaseApi.get('/Games/GetAllForAutoComplete'); },
    create(data) { return creatorsBaseApi.postWithFormData('/Games/Create', data); },
    edit(data) { return creatorsBaseApi.putWithFormData('/Games/Edit', data); },
    delete(id) { return creatorsBaseApi.delete('/Games/Delete', id); } 
};