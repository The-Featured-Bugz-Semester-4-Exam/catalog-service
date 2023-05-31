# The Featured Bugz - Catalog Service

Welcome to the README for the Catalog Service project developed by The Featured Bugz team. This project consists of two main projects: the API testing project and the catalogServiceAPI project. The catalogServiceAPI provides functionality for managing a catalog of items. It offers various API endpoints to retrieve, create, update, and delete items from a MongoDB database. Additionally, it interacts with another service called [auction-service](https://github.com/The-Featured-Bugz-Semester-4-Exam/auction-service.git) for auction-related functionality.

## Table of Contents
- Overview
- API Endpoints
 - Get Version
 - Get All Items
 - Get Item
 - Post Item
 - Delete Item
 - Post Auction
 - Get Auction Price
- Prerequisites
- Getting Started
    - Installation
    - Configuration
- Testing with Postman
- Contributing

## Overview

The Catalog Service project provides an API for managing a catalog of items. It consists of the catalogServiceAPI project, which offers various API endpoints. These endpoints allow you to perform actions such as retrieving the catalog version, getting all items, getting a specific item by its ID, adding new items, deleting items, updating items, posting items for auction, and getting the current auction price for a specific item.

## **API EndPoints**
## Get Version
```bash
GET /api/getVersion
```
This endpoint allows you to retrieve the version information of the catalogServiceAPI project.

**Response**
The API will respond with the version information of the project.

## Get All Items
```bash
Get /api/getAllItems
```
This endpoint returns all items in the catalog stored in the MongoDB database.

**Response**

If some Items are found it will respond with **`OK 200`** and a list of items. If
not found it will respond with **`404 Not Found`**


## Get Item
```bash
Get /api/getItem/{id}
```
The endpoint return a specific item based on a id.

**Reponse**

If item is found it will respond with **`Ok 200`** with a item. If not
found it will respond with **`404 Not Found`**

**Example on a respond succes**
```json
{
  "_id": {
    "$oid": "647091e613220effbc797be7"
  },
  "ItemID": 1,
  "ItemName": "Navnet på dit produkt",
  "ItemDescription": "Beskrivelse af dit produkt",
  "ItemStartPrice": 100,
  "ItemCurrentBid": -1,
  "ItemSellerID": 123,
  "ItemStartDate": {
    "$date": "2023-05-25T00:00:00.000Z"
  },
  "ItemEndDate": {
    "$date": "2023-05-30T00:00:00.000Z"
  }
}
```
## PostItem
```bash
POST api/postItem
```
This endpoint is used to add a new item to the catalog.

**Response** 

If the post is succes it will respond with **`Ok 200``**. If failed
it will respond with **`Not Modified 304`**.

**Request body**

Example on how the body should look like
```json
{
  "ItemID": 1,
  "ItemName": "Navnet på dit produkt",
  "ItemDescription": "Beskrivelse af dit produkt",
  "ItemStartPrice": 100,
  "ItemCurrentBid": -1,
  "ItemSellerID": 123,
  "ItemStartDate": "2023-06-15T18:00:00Z",
  "ItemEndDate": "2023-05-30T00:00:00.000Z",
  
}
```
