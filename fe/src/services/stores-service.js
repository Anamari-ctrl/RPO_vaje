import { Store } from '../models/store';

const MOCK_STORES = [
  new Store(
    1,
    'The Three Owls - Ljubljana Center',
    {
      monday: '9:00 - 20:00',
      tuesday: '9:00 - 20:00',
      wednesday: '9:00 - 20:00',
      thursday: '9:00 - 20:00',
      friday: '9:00 - 21:00',
      saturday: '10:00 - 18:00',
      sunday: 'Closed'
    },
    {
      phone: '+386 1 234 5678',
      email: 'ljubljana@threeowls.si'
    },
    {
      name: 'Ana Novak',
      email: 'ana.novak@threeowls.si'
    },
    [
      'Extended hours during December',
      'Book signing event on December 20th',
      'Free gift wrapping available'
    ],
    {
      lat: 46.0569,
      lng: 14.5058,
      address: 'Čopova ulica 5, 1000 Ljubljana'
    }
  ),
  new Store(
    2,
    'The Three Owls - Maribor',
    {
      monday: '9:00 - 19:00',
      tuesday: '9:00 - 19:00',
      wednesday: '9:00 - 19:00',
      thursday: '9:00 - 19:00',
      friday: '9:00 - 20:00',
      saturday: '9:00 - 17:00',
      sunday: 'Closed'
    },
    {
      phone: '+386 2 345 6789',
      email: 'maribor@threeowls.si'
    },
    {
      name: 'Marko Horvat',
      email: 'marko.horvat@threeowls.si'
    },
    [
      'Children\'s reading hour every Saturday at 11:00',
      'Student discount 10%'
    ],
    {
      lat: 46.5547,
      lng: 15.6459,
      address: 'Glavni trg 12, 2000 Maribor'
    }
  ),
  new Store(
    3,
    'The Three Owls - Celje',
    {
      monday: '9:00 - 19:00',
      tuesday: '9:00 - 19:00',
      wednesday: '9:00 - 19:00',
      thursday: '9:00 - 19:00',
      friday: '9:00 - 19:00',
      saturday: '9:00 - 15:00',
      sunday: 'Closed'
    },
    {
      phone: '+386 3 456 7890',
      email: 'celje@threeowls.si'
    },
    {
      name: 'Nina Kovač',
      email: 'nina.kovac@threeowls.si'
    },
    [
      'Now accepting used book donations',
      'Book club meetings every Wednesday at 18:00'
    ],
    {
      lat: 46.2396,
      lng: 15.2677,
      address: 'Prešernova ulica 8, 3000 Celje'
    }
  ),
  new Store(
    4,
    'The Three Owls - Koper',
    {
      monday: '9:00 - 18:00',
      tuesday: '9:00 - 18:00',
      wednesday: '9:00 - 18:00',
      thursday: '9:00 - 18:00',
      friday: '9:00 - 20:00',
      saturday: '10:00 - 16:00',
      sunday: 'Closed'
    },
    {
      phone: '+386 5 567 8901',
      email: 'koper@threeowls.si'
    },
    {
      name: 'Petra Zupan',
      email: 'petra.zupan@threeowls.si'
    },
    [
      'Multilingual section expanded',
      'Summer reading program starting June'
    ],
    {
      lat: 45.5481,
      lng: 13.7301,
      address: 'Titov trg 3, 6000 Koper'
    }
  ),
  new Store(
    5,
    'The Three Owls - Kranj',
    {
      monday: '9:00 - 19:00',
      tuesday: '9:00 - 19:00',
      wednesday: '9:00 - 19:00',
      thursday: '9:00 - 19:00',
      friday: '9:00 - 19:00',
      saturday: '9:00 - 16:00',
      sunday: 'Closed'
    },
    {
      phone: '+386 4 678 9012',
      email: 'kranj@threeowls.si'
    },
    {
      name: 'Luka Petrič',
      email: 'luka.petric@threeowls.si'
    },
    [
      'New fantasy section now open',
      'Author meet & greet monthly'
    ],
    {
      lat: 46.2437,
      lng: 14.3561,
      address: 'Glavni trg 4, 4000 Kranj'
    }
  ),
  new Store(
    6,
    'The Three Owls - Nova Gorica',
    {
      monday: '9:00 - 18:00',
      tuesday: '9:00 - 18:00',
      wednesday: '9:00 - 18:00',
      thursday: '9:00 - 18:00',
      friday: '9:00 - 19:00',
      saturday: '9:00 - 14:00',
      sunday: 'Closed'
    },
    {
      phone: '+386 5 789 0123',
      email: 'nova.gorica@threeowls.si'
    },
    {
      name: 'Sara Mlakar',
      email: 'sara.mlakar@threeowls.si'
    },
    [
      'Italian language books available',
      'Cross-border delivery service'
    ],
    {
      lat: 45.9561,
      lng: 13.6477,
      address: 'Bevkov trg 6, 5000 Nova Gorica'
    }
  )
];

class StoresService {
  getStores() {
    // Simulate API delay
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve({
          stores: MOCK_STORES,
          total: MOCK_STORES.length
        });
      }, 300);
    });
  }

  getStoreById(id) {
    return new Promise((resolve) => {
      setTimeout(() => {
        const store = MOCK_STORES.find(s => s.id === parseInt(id));
        resolve(store);
      }, 200);
    });
  }
}

export default new StoresService();
