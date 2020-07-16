import baseApi from '../base-api';

// TODO [GM]: Extract in environment file.
var baseUrl = 'http://localhost:5005/api';

export default {
    getGameDetails(id) { return baseApi.get(baseUrl + '/Games/GetGameDetails', { id: id }); },
    getAllSearchGames() { return baseApi.get(baseUrl + '/Games/GetAllSearchGames'); },
};