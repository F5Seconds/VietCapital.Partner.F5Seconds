// Import the functions you need from the SDKs you need
import {initializeApp} from 'firebase/app';
import {getStorage, ref} from 'firebase/storage';
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
const firebaseConfig = {
  apiKey: 'AIzaSyCRXmkFT1jfghdEtfQQtUMHLC3EXlSSOVs',
  authDomain: 'vietcapitalstorage.firebaseapp.com',
  projectId: 'vietcapitalstorage',
  storageBucket: 'vietcapitalstorage.appspot.com',
  messagingSenderId: '330302205051',
  appId: '1:330302205051:web:66137b13d9c51a4896c6de',
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);

const storage = getStorage(app);
const storageRef = ref(storage);

export {storageRef, storage};
