using System;
using System.Collections.Generic;
using System.Text;

namespace Authorization.Contracts
{
    interface IPersonGroup
    {

        /// <summary>PersonGroup - Create</summary>
        /// <param name="personGroupId">User-provided personGroupId as a string. The valid characters include numbers, English letters in lower case, '-' and '_'. The maximum length of the personGroupId is 64.</param>
        /// <param name="body">JSON fields in request body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>name</td><th>String</th><td>Person group display name. The maximum length is 128.</td></tr>
        /// <tr><td>userData (optional)</td><th>String</th><td>User-provided data attached to the person group. The size limit is 16KB.</td></tr>
        /// 
        /// </tbody>
        /// </table></param>
        /// <returns>A successful call returns an empty response body.</returns>
        System.Threading.Tasks.Task CreatePersonGroupAsync(string personGroupId, object body);

        /// <summary>PersonGroup - Delete</summary>
        /// <param name="personGroupId">The personGroupId of the person group to be deleted.</param>
        /// <returns>A successful call returns an empty response body.</returns>
        System.Threading.Tasks.Task DeletePErsonGroupAsync(string personGroupId);

        /// <summary>PersonGroup - Get</summary>
        /// <param name="personGroupId">personGroupId of the target person group.</param>
        /// <returns>A successful call returns the person group's information.
        /// <br/><br/> JSON fields in response body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>personGroupId</td><th>String</th><td>Target personGroupId provided in request parameter.</td></tr>
        /// <tr><td>name</td><th>String</th><td>Person group's display name.</td></tr>
        /// <tr><td>userData</td><th>String</th><td>User-provided data attached to this person group.</td></tr>
        /// 
        /// </tbody>
        /// </table></returns>
        System.Threading.Tasks.Task GetPersonGroupAsync(string personGroupId);

        /// <summary>PersonGroup - Update</summary>
        /// <param name="personGroupId">personGroupId of the person group to be updated.</param>
        /// <param name="body">JSON fields in request body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>name</td><th>String</th><td>Person group display name. The maximum length is 128.</td></tr>
        /// <tr><td>userData</td><th>String</th><td>User-provided data attached to the person group. The size limit is 16KB.</td></tr>
        /// 
        /// </tbody>
        /// </table></param>
        /// <returns>A successful call returns an empty response body.</returns>
        System.Threading.Tasks.Task UpdatePersonGroupAsync(string personGroupId, object body);

        /// <summary>PersonGroup - Get Training Status</summary>
        /// <param name="personGroupId">personGroupId of target person group.</param>
        /// <returns>A successful call returns the person group's training status.
        /// <br />
        /// <br /> JSON fields in response body:
        /// <table class="element table">
        ///     <thead>
        ///         <tr>
        ///             <th>Fields</th>
        ///             <th>Type</th>
        ///             <th>Description</th>
        ///         </tr>
        ///     </thead>
        ///     <tbody>
        ///         <tr>
        ///             <td>status</td>
        ///             <th>String</th>
        ///             <td>Training status: notstarted, running, succeeded, failed. If the person group has never been trained before, the
        ///                 status is notstarted. If the training is ongoing, the status is running. Status succeed means this person
        ///                 group is ready for
        ///                 <a href="/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395239">Face - Identify</a>. Status failed is often caused by no person or no persisted face exist in the person
        ///                 group. </td>
        ///         </tr>
        ///         <tr>
        ///             <td>createdDateTime</td>
        ///             <th>String</th>
        ///             <td>A combined UTC date and time string that describes the last time when the person group is requested to train.
        ///                 E.g. 2018-10-15T11:51:27.6872495Z.
        ///             </td>
        ///         </tr>
        ///         <tr>
        ///             <td>lastActionDateTime</td>
        ///             <th>String</th>
        ///             <td>A combined UTC date and time string that describes the last time the person group's training status was modifed.
        ///                 E.g. 2018-10-15T11:51:27.8705696Z.</td>
        ///         </tr>
        ///         <tr>
        ///             <td>message</td>
        ///             <th>String</th>
        ///             <td>Show failure message when training failed (omitted when training succeed).</td>
        ///         </tr>
        ///     </tbody>
        /// </table></returns>
        System.Threading.Tasks.Task GetTrainingStatusAsync(string personGroupId);

    }
}
