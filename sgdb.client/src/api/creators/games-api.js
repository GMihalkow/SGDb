import creatorsBaseApi from './creators-base-api';

export default {
    getGameIndexCards() { return creatorsBaseApi.get('/Games/GetGameIndexCards'); }
}