using System;
using System.Collections.Generic;
using System.Text;

namespace Authorization.Contracts
{
    interface IPerson
    {
        /// <summary>PersonGroup Person - Create</summary>
        /// <param name="personGroupId">Specifying the target person group to create the person.</param>
        /// <param name="body">JSON fields in request body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>name</td><th>String</th><td>Display name of the target person. The maximum length is 128.</td></tr>
        /// <tr><td>userData (optional)</td><th>String</th><td>Optional fields for user-provided data attached to a person. Size limit is 16KB.</td></tr>
        /// 
        /// </tbody>
        /// </table></param>
        /// <returns>A successful call returns a new personId created.
        /// <br/><br/>
        /// JSON fields in response body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>personId</td><th>String</th><td>personID of the new created person.</td></tr>
        /// 
        /// </tbody>
        /// </table></returns>
        System.Threading.Tasks.Task CreatePersonGroupAsync(string personGroupId, object body);

        /// <summary>PersonGroup Person - List</summary>
        /// <param name="personGroupId">personGroupId of the target person group.</param>
        /// <param name="start">List persons from the least personId greater than the "start". It contains no more than 64 characters. Default is empty.</param>
        /// <param name="top">The number of persons to list, ranging in [1, 1000]. Default is 1000.</param>
        /// <returns>A successful call returns an array of person information that belong to the person group.
        /// <br/><br/> JSON fields in response body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>personId</td><th>String</th><td>personId of the person in the person group.</td></tr>
        /// <tr><td>name</td><th>String</th><td>Person's display name.</td></tr>
        /// <tr><td>userData</td><th>String</th><td>User-provided data attached to the person.</td></tr>
        /// <tr><td>persistedFaceIds</td><th>Array</th><td>persistedFaceId array of registered faces of the person.</td></tr>
        /// </tbody>
        /// </table></returns>
        System.Threading.Tasks.Task GetPersonsAsync(string personGroupId, string start, Top top);

        /// <summary>PersonGroup Person - Delete</summary>
        /// <param name="personGroupId">Specifying the person group containing the person.</param>
        /// <param name="personId">The target personId to delete.</param>
        /// <returns>A successful call returns an empty response body.</returns>
        System.Threading.Tasks.Task DeletePersonAsync(string personGroupId, string personId);

        /// <summary>PersonGroup Person - Get</summary>
        /// <param name="personGroupId">Specifying the person group containing the target person.</param>
        /// <param name="personId">Specifying the target person.</param>
        /// <returns>A successful call returns the person's information.
        /// <br/><br/> JSON fields in response body:
        /// 
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>personId</td><th>String</th><td>personId of the retrieved person.</td></tr>
        /// <tr><td>persistedFaceIds</td><th>Array</th><td>persistedFaceIds of registered faces in the person. These persistedFaceIds are returned from <a href="/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f3039523b">PersonGroup PersonFace - Add</a>, and will not expire.</td></tr>
        /// <tr><td>name</td><th>String</th><td>Person's display name.</td></tr>
        /// <tr><td>userData</td><th>String</th><td>User-provided data attached to the person.</td></tr>
        /// </tbody>
        /// </table></returns>
        System.Threading.Tasks.Task GetPersonAsync(string personGroupId, string personId);

        /// <summary>PersonGroup Person - Update</summary>
        /// <param name="personGroupId">Specifying the person group containing the target person.</param>
        /// <param name="personId">personId of the target person.</param>
        /// <param name="body">JSON fields in request body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>name</td><th>String</th><td>Target person's display name. Maximum length is 128.</td></tr>
        /// <tr><td>userData</td><th>String</th><td>User-provided data attached to the person. Maximum length is 16KB.</td></tr>
        /// 
        /// </tbody>
        /// </table></param>
        /// <returns>A successful call returns an empty response body.</returns>
        System.Threading.Tasks.Task UpdatePersonAsync(string personGroupId, string personId, object body);

    }
}
