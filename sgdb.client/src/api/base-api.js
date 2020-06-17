import axios from 'axios';

var httpClient = axios.create({
    baseURL: 'https://localhost:5001',
    timeout: 10000, // indicates, 1000ms ie. 1 second
    headers: {
        'Content-Type': 'application/json',
        'Access-Control-Allow-Headers' : '*'
    }
});

export default {
    getAll: function() { 
        return httpClient.get('api/Games/GetAll');
    },
    getAllForAutoComplete: async function() {
        return await httpClient.get('api/Games/GetAllForAutoComplete');
    }
};