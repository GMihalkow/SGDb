import creatorsBaseApi from './creators-base-api';

export default {
    getAllGenresForMultiselect() {
        return creatorsBaseApi.get('/Genres/GetAllGenresForMultiselect');
    }
};