using System;
using System.Collections.Generic;
using System.Text;

namespace Authorization.Contracts
{
    interface IFace
    {
        /// <summary>FaceList - Add Face</summary>
        /// <param name="faceListId">Valid character is letter in lower case or digit or '-' or '_', maximum length is 64.</param>
        /// <param name="userData">User-specified data about the face list for any purpose. The maximum length is 1KB.</param>
        /// <param name="targetFace">A face rectangle to specify the target face to be added into the face list, in the format of "targetFace=left,top,width,height". E.g. "targetFace=10,10,100,100". If there is more than one face in the image, targetFace is required to specify which face to add. No targetFace means there is only one face detected in the entire image.</param>
        /// <param name="body">JSON fields in request body:
        /// <table class="element table">
        /// 	<thead>
        /// 		<tr>
        /// 			<th>Fields</th>
        /// 			<th>Type</th>
        /// 			<th>Description</th>
        /// 		</tr>
        /// 	</thead>
        /// 	<tbody>
        /// 		<tr>
        /// 			<td>url</td>
        /// 			<th>String</th>
        /// 			<td>Image url. Image file size should be between 1KB and 6MB. Only one face is allowed per image.</td>
        /// 		</tr>
        /// 	</tbody>
        /// </table></param>
        /// <returns>A successful call returns a new persistedFaceId.
        /// <br/><br/> JSON fields in response body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>persistedFaceId</td><th>String</th><td>persistedFaceId of the added face, which is persisted and will not expire. Different from faceId which is created in <a href="/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236">Face - Detect</a> and will expire in 24 hours after the detection call.</td></tr>
        /// </tbody>
        /// </table></returns>
        System.Threading.Tasks.Task AddFaceAsync(string faceListId, string userData, string targetFace, object body);

        /// <summary>FaceList - Create</summary>
        /// <param name="faceListId">Valid character is letter in lower case or digit or '-' or '_', maximum length is 64.</param>
        /// <param name="body">JSON fields in request body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>name</td><th>String</th><td>Name of the created face list, maximum length is 128.</td></tr>
        /// <tr><td>userData (optional)</td><th>String</th><td>Optional user defined data for the face list. Length should not exceed 16KB.</td></tr>
        /// 
        /// </tbody>
        /// </table></param>
        /// <returns>A successful call returns an empty response body.</returns>
        System.Threading.Tasks.Task CreateIndividualAsync(string faceListId, object body);

        /// <summary>FaceList - Delete</summary>
        /// <param name="faceListId">Valid character is letter in lower case or digit or '-' or '_', maximum length is 64.</param>
        /// <returns>A successful call returns an empty response body.</returns>
        System.Threading.Tasks.Task DeleteIndividualAsync(string faceListId);

        /// <summary>FaceList - Get</summary>
        /// <param name="faceListId">Valid character is letter in lower case or digit or '-' or '_', maximum length is 64.</param>
        /// <returns>A successful call returns the face list's information.
        /// <br/><br/> JSON fields in response body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>faceListId</td><th>String</th><td>faceListId of the target face list.</td></tr>
        /// <tr><td>name</td><th>String</th><td>Face list's display name.</td></tr>
        /// <tr><td>userData</td><th>String</th><td>User-provided data attached to this face list.</td></tr>
        /// <tr><td>persistedFaces</td><th>Array</th><td>Faces in the face list.</td></tr>
        /// </tbody>
        /// </table></returns>
        System.Threading.Tasks.Task GetIndividualAsync(string faceListId);

        /// <summary>FaceList - Update</summary>
        /// <param name="faceListId">Valid character is letter in lower case or digit or '-' or '_', maximum length is 64.</param>
        /// <param name="body">JSON fields in request body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>name</td><th>String</th><td>Name of the face list, maximum length is 128</td></tr>
        /// <tr><td>userData (optional)</td><th>String</th><td>Optional user defined data for the face list. Length should not exceed 16KB</td></tr>
        /// </tbody>
        /// </table></param>
        /// <returns>A successful call returns an empty response body.</returns>
        System.Threading.Tasks.Task UpdateIndividualAsync(string faceListId, object body);

        /// <summary>FaceList - Delete Face</summary>
        /// <param name="faceListId">faceListId of an existing face list. Valid character is letter in lower case or digit or '-' or '_', maximum length is 64.</param>
        /// <param name="persistedFaceId">persistedFaceId of an existing face. Valid character is letter in lower case or digit or '-' or '_', maximum length is 64.</param>
        /// <returns>A successful call returns an empty response body.</returns>
        System.Threading.Tasks.Task DeleteIndividualAsync(string faceListId, string persistedFaceId);

        /// <summary>FaceList - List</summary>
        /// <returns>A successful call returns an array of faceList.
        /// <br /><br /> JSON fields in response body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>faceListId</td><th>String</th><td>Face list ID</td></tr>
        /// <tr><td>name</td><th>String</th><td>Face list name which user assigned</td></tr>
        /// <tr><td>userData</td><th>String</th><td>User-provided data attached to the face list</td></tr>
        /// 
        /// </tbody>
        /// </table></returns>
        System.Threading.Tasks.Task ListFacesAsync();

        /// <summary>PersonGroup Person - Add Face</summary>
        /// <param name="personGroupId">Specifying the person group containing the target person.</param>
        /// <param name="personId">Target person that the face is added to.</param>
        /// <param name="userData">User-specified data about the target face to add for any purpose. The maximum length is 1KB.</param>
        /// <param name="targetFace">A face rectangle to specify the target face to be added to a person, in the format of "targetFace=left,top,width,height". E.g. "targetFace=10,10,100,100". If there is more than one face in the image, targetFace is required to specify which face to add. No targetFace means there is only one face detected in the entire image.</param>
        /// <param name="body"><br/><br/> JSON fields in request body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>url</td><th>String</th><td>Face image URL. Valid image size is from 1KB to 6MB. Only one face is allowed per image.</td></tr>
        /// 
        /// </tbody>
        /// </table></param>
        /// <returns>A successful call returns the new persistedFaceId.
        /// <br/><br/> JSON fields in response body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>persistedFaceId</td><th>String</th><td>persistedFaceId of the added face, which is persisted and will not expire. Different from faceId which is created in <a href="/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236">Face - Detect</a> and will expire in 24 hours after the detection call. </td></tr>
        /// </tbody>
        /// </table></returns>
        System.Threading.Tasks.Task AddFaceAsync(string personGroupId, string personId, string userData, string targetFace, object body);


        /// <summary>PersonGroup Person - Delete Face</summary>
        /// <param name="personGroupId">Specifying the person group containing the target person.</param>
        /// <param name="personId">Specifying the person that the target persisted face belong to.</param>
        /// <param name="persistedFaceId">The persisted face to remove. This persistedFaceId is returned from <a href="/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f3039523b">PersonGroup PersonFace - Add</a>.</param>
        /// <returns>A successful call returns an empty response body.</returns>
        System.Threading.Tasks.Task DeleteFaceAsync(string personGroupId, string personId, string persistedFaceId);

        /// <summary>PersonGroup Person - Get Face</summary>
        /// <param name="personGroupId">Specifying the person group containing the target person.</param>
        /// <param name="personId">Specifying the target person that the face belongs to.</param>
        /// <param name="persistedFaceId">The persistedFaceId of the target persisted face of the person.</param>
        /// <returns>A successful call returns target persisted face's information (persistedFaceId and userData).
        /// <br/><br/>
        /// JSON fields in response body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>persistedFaceId</td><th>String</th><td>The persistedFaceId of the target face, which is persisted and will not expire. Different from faceId created by <a href="/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236">Face - Detect</a> and will expire in 24 hours after the detection call.</td></tr>
        /// <tr><td>userData</td><th>String</th><td>User-provided data attached to the face.</td></tr>
        /// 
        /// </tbody>
        /// </table></returns>
        System.Threading.Tasks.Task GetFaceAsync(string personGroupId, string personId, string persistedFaceId);

        /// <summary>PersonGroup Person - Update Face</summary>
        /// <param name="personGroupId">Specifying the person group containing the target person.</param>
        /// <param name="personId">personId of the target person.</param>
        /// <param name="persistedFaceId">persistedFaceId of target face, which is persisted and will not expire.</param>
        /// <param name="body">JSON fields in request body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>userData (optional)</td><th>String</th><td>Optional. Attach userData to person's persisted face. The size limit is 1KB.</td></tr>
        /// 
        /// </tbody>
        /// </table></param>
        /// <returns>A successful call returns an empty response body.</returns>
        System.Threading.Tasks.Task UpdateFaceAsync(string personGroupId, string personId, string persistedFaceId, object body);


    }
}
