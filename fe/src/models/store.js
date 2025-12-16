export class Store {
  constructor(
    id,
    title,
    workingHours,
    contact,
    manager,
    notices,
    location
  ) {
    this.id = id;
    this.title = title;
    this.workingHours = workingHours; // { monday: '9:00 - 18:00', ... }
    this.contact = contact; // { phone: '...', email: '...' }
    this.manager = manager; // { name: '...', email: '...' }
    this.notices = notices; // array of notice strings
    this.location = location; // { lat: number, lng: number, address: '...' }
  }
}
