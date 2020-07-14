import creatorsBaseApi from './creators-base-api';

export default {
    getAllSearchPublishers() {
        return creatorsBaseApi.get('/Publishers/GetAllSearchPublishers');
    },
    getAllPublishersForMultiselect() {
        return creatorsBaseApi.get('/Publishers/GetAllPublishersForMultiselect');
    },
    create(data) {
        return creatorsBaseApi.postWithFormData('/Publishers/Create', data);
    },
    edit(data) {
        return creatorsBaseApi.patch('/Publishers/Edit', data);
    },
    delete(id) {
        return creatorsBaseApi.delete('/Publishers/Delete', id);
    }
};