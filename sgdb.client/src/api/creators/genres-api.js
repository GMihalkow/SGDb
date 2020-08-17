import creatorsBaseApi from './creators-base-api';

export default {
    getAllGenresForMultiselect() {
        return creatorsBaseApi.get('/Genres/GetAllGenresForMultiselect');
    },
    getAllSearchGenres() {
        return creatorsBaseApi.get('/Genres/GetAllSearchGenres');
    },
    create(data) {
        return creatorsBaseApi.postWithFormData('/Genres/Create', data);
    },
    edit(data) {
        return creatorsBaseApi.patch('/Genres/Edit', data);
    },
    delete(data) {
        return creatorsBaseApi.delete('/Genres/Delete', data);
    }
};