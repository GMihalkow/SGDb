import creatorsBaseApi from './creators-base-api';

export default {
    getAllPublishersForMultiselect() {
        return creatorsBaseApi.get('/Publishers/GetAllPublishersForMultiselect');
    }
};