// Import the functions you need from the SDKs you need
import {initializeApp} from 'firebase/app';
import {getStorage, ref} from 'firebase/storage';
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
const firebaseConfig = {
  apiKey: 'AIzaSyCUdViRNyTMrR_zZOd34dIrZVGYdZ-V-JA',
  authDomain: 'core-loyalty.firebaseapp.com',
  databaseURL: 'https://core-loyalty-default-rtdb.asia-southeast1.firebasedatabase.app',
  projectId: 'core-loyalty',
  storageBucket: 'core-loyalty.appspot.com',
  messagingSenderId: '84291535157',
  appId: '1:84291535157:web:4bc387dc15d42d393b4c7c',
  measurementId: 'G-T6MNPRZP9S',
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);

const storage = getStorage(app);
const storageRef = ref(storage);

export {storageRef, storage};
