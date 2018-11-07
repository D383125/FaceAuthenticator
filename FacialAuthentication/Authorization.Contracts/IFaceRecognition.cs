using System;
using System.Collections.Generic;
using System.Text;

namespace Authorization.Contracts
{
    interface IFaceRecognition
    {
        /// <summary>Face - Detect</summary>
        /// <param name="returnFaceId">Return faceIds of the detected faces or not. The default value is true.</param>
        /// <param name="returnFaceLandmarks">Return face landmarks of the detected faces or not. The default value is false.</param>
        /// <param name="returnFaceAttributes">Analyze and return the one or more specified face attributes in the comma-separated string like "returnFaceAttributes=age,gender". Supported face attributes include age, gender, headPose, smile, facialHair, glasses, emotion, hair, makeup, occlusion, accessories, blur, exposure and noise. Face attribute analysis has additional computational and time cost.</param>
        /// <param name="body"><article class="ed_api_param">
        /// To detect in a URL (or binary data) specified image.
        /// <br/><br/> JSON fields in the request body: <br />
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>url</td><th>String</th><td>URL of input image.</td></tr>
        /// 
        /// </tbody>
        /// </table></param>
        /// <returns>A successful call returns an array of face entries ranked by face rectangle size in descending order. An empty response indicates
        /// no faces detected. A face entry may contain the following values depending on input parameters:
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
        ///             <td>faceId</td>
        ///             <th>String</th>
        ///             <td>Unique faceId of the detected face, created by detection API and it will expire 24 hours after the detection call.
        ///                 To return this, it requires "returnFaceId" parameter to be true.</td>
        ///         </tr>
        ///         <tr>
        ///             <td>faceRectangle</td>
        ///             <th>Object</th>
        ///             <td>A rectangle area for the face location on image.</td>
        ///         </tr>
        ///         <tr>
        ///             <td>faceLandmarks</td>
        ///             <th>Object</th>
        ///             <td>An array of 27-point face landmarks pointing to the important positions of face components. To return this, it
        ///                 requires "returnFaceLandmarks" parameter to be true.</td>
        ///         </tr>
        ///         <tr>
        ///             <td>faceAttributes</td>
        ///             <th>Object</th>
        ///             <td> Face Attributes:
        ///                 <ul>
        ///                     <li>age: an estimated "visual age" number in years. It is how old a person looks like rather than the actual biological age.</li>
        ///                     <li>gender: male or female.</li>
        ///                     <li>smile: smile intensity, a number between [0,1].</li>
        ///                     <li>facialHair: return lengths in three facial hair areas: moustache, beard and sideburns. The length is
        ///                         a number between [0,1]. 0 for no facial hair in this area, 1 for long or very thick facial hairs
        ///                         in this area.</li>
        ///                     <li>headPose: 3-D roll/yaw/pitch angles for face direction. Note, Pitch value is a reserved field and will
        ///                         always return 0.</li>
        ///                     <li>glasses: glasses type. Values include 'NoGlasses', 'ReadingGlasses', 'Sunglasses', 'SwimmingGoggles'.</li>
        ///                     <li>emotion: emotion intensity, including neutral, anger, contempt, disgust, fear, happiness, sadness and
        ///                         surprise.
        ///                     </li>
        ///                     <li>hair: group of hair values indicating whether the hair is visible, bald, and hair color if hair is visible.</li>
        ///                     <li>makeup: whether eye, lip areas are made-up or not.</li>
        ///                     <li>accessories: accessories around face, including 'headwear', 'glasses' and 'mask'. Empty array means no
        ///                         accessories detected. Note this is after a face is detected. Large mask could result in no face to
        ///                         be detected.</li>
        ///                     <li>blur: face is blurry or not. Level returns 'Low', 'Medium' or 'High'. Value returns a number between
        ///                         [0,1], the larger the blurrier.</li>
        ///                     <li>exposure: face exposure level. Level returns 'GoodExposure', 'OverExposure' or 'UnderExposure'.</li>
        ///                     <li>noise: noise level of face pixels. Level returns 'Low', 'Medium' and 'High'. Value returns a number between
        ///                         [0,1], the larger the noisier</li>
        ///                 </ul>
        ///             </td>
        ///         </tr>
        ///     </tbody>
        /// </table></returns>
        System.Threading.Tasks.Task DetectFaceAsync(bool returnFaceId, bool returnFaceLandmarks, string returnFaceAttributes, object body);

        /// <summary>Face - Find Similar</summary>
        /// <param name="body">JSON fields in request body:
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
        ///             <td>faceId</td>
        ///             <th>String</th>
        ///             <td>faceId of the query face. User needs to call <a href="/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236">Face - Detect</a> first to get a valid faceId. Note that this faceId is not persisted and will expire 24 hours after the detection call.</td>
        ///         </tr>
        ///         <tr>
        ///             <td>faceListId</td>
        ///             <th>String</th>
        ///             <td>An existing user-specified unique candidate face list, created in <a href="/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f3039524b">FaceList - Create</a>. Face list contains a set of persistedFaceIds which are persisted and will never expire. Parameter faceListId, largeFaceListId and faceIds should not be provided at the same time.</td>
        ///         </tr>
        ///         <tr>
        ///             <td>largeFaceListId</td>
        ///             <th>String</th>
        ///             <td>An existing user-specified unique candidate large face list, created in <a href="/docs/services/563879b61984550e40cbbe8d/operations/5a157b68d2de3616c086f2cc">LargeFaceList - Create</a>. Large face list contains a set of persistedFaceIds which are persisted and will never expire. Parameter faceListId, largeFaceListId and faceIds should not be provided at the same time.</td>
        ///         </tr>
        ///         <tr>
        ///             <td>faceIds</td>
        ///             <th>Array</th>
        ///             <td>An array of candidate faceIds. All of them are created by <a href="/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236">Face - Detect</a> and the faceIds will expire 24 hours after the detection call. The number of faceIds is limited to 1000. Parameter faceListId, largeFaceListId and faceIds should not be provided at the same time.</td>
        ///         </tr>
        ///         <tr>
        ///             <td>maxNumOfCandidatesReturned (optional)</td>
        ///             <th>Number</th>
        ///             <td>Optional parameter.
        ///                 <br /> The number of top similar faces returned.
        ///                 <br /> The valid range is [1, 1000].It defaults to 20. </td>
        ///         </tr>
        ///         <tr>
        ///             <td>mode (optional)</td>
        ///             <th>String</th>
        ///             <td>Optional parameter.
        ///                 <br /> Similar face searching mode. It can be "matchPerson" or "matchFace". It defaults to "matchPerson".</td>
        ///         </tr>
        ///     </tbody>
        /// </table></param>
        /// <returns>A successful call returns an array of the most similar faces represented in faceId if the input parameter is faceIds or persistedFaceId if the input parameter is faceListId or largeFaceListId.
        /// <br /><br /> JSON fields in response body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>persistedFaceId</td><th>String</th><td>persistedFaceId of candidate face when find by faceListId or largeFaceListId. persistedFaceId in face list/large face list is persisted and will not expire. As showed in below response.</td></tr>
        /// <tr><td>faceId</td><th>String</th><td>faceId of candidate face when find by faceIds. faceId is created by <a href="/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236">Face - Detect</a> and will expire 24 hours after the detection call. </td></tr>
        /// <tr><td>confidence</td><th>Number</th><td>Similarity confidence of the candidate face. The higher confidence, the more similar. Range between [0,1].</td></tr>
        /// </tbody>
        /// </table></returns>
        System.Threading.Tasks.Task FindSimilarFaceAsync(object body);

        /// <summary>Face - Group</summary>
        /// <param name="body">JSON fields in request body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>faceIds</td><th>Array</th><td>Array of candidate faceId created by <a href="/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236">Face - Detect</a>. The maximum is 1000 faces.</td></tr>
        /// 
        /// </tbody>
        /// </table></param>
        /// <returns>A successful call returns one or more groups of similar faces (rank by group size) and a messyGroup.
        /// <br /><br /> JSON fields in response body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>groups</td><th>Array</th><td>A partition of the original faces based on face similarity. Groups are ranked by number of faces.</td></tr>
        /// <tr><td>messyGroup</td><th>Array</th><td>Face ids array of faces that cannot find any similar faces from original faces.</td></tr>
        /// </tbody>
        /// </table></returns>
        System.Threading.Tasks.Task IdentityGroupAsync(object body);

        /// <summary>Face - Identify</summary>
        /// <param name="body">JSON fields in request body:
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
        ///             <td>faceIds</td>
        ///             <th>Array</th>
        ///             <td> Array of query faces faceIds, created by the <a href="/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236">Face - Detect</a>. Each of the faces are identified independently. The valid number of faceIds is between [1, 10].</td>
        ///         </tr>
        ///         <tr>
        ///             <td>personGroupId</td>
        ///             <th>String</th>
        ///             <td>personGroupId of the target person group, created by <a href="/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395244">PersonGroup - Create</a>. Parameter personGroupId and largePersonGroupId should not be provided at the same time.</td>
        ///         </tr>
        ///         <tr>
        ///             <td>largePersonGroupId</td>
        ///             <th>String</th>
        ///             <td>largePersonGroupId of the target large person group, created by <a href="/docs/services/563879b61984550e40cbbe8d/operations/599acdee6ac60f11b48b5a9d">LargePersonGroup - Create</a>. Parameter personGroupId and largePersonGroupId should not be provided at the same time.</td>
        ///         </tr>
        ///         <tr>
        ///             <td>maxNumOfCandidatesReturned (optional)</td>
        ///             <th>Number</th>
        ///             <td>The range of maxNumOfCandidatesReturned is between 1 and 100 (default is 10).</td>
        ///         </tr>
        ///         <tr>
        ///             <td>confidenceThreshold (optional)</td>
        ///             <th>Number</th>
        ///             <td>Optional parameter.
        ///                 <br /> Customized identification confidence threshold, in the range of [0, 1]. Advanced user can tweak this value to override default internal threshold for better precision on their scenario data. Note there is no guarantee of this threshold value working on other data and after algorithm updates.</td>
        ///         </tr>
        ///     </tbody>
        /// </table></param>
        /// <returns>A successful call returns the identified candidate person(s) for each query face.
        /// <br/><br/> JSON fields in response body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>faceId</td><th>String</th><td>faceId of the query face.</td></tr>
        /// <tr><td>candidates</td><th>Array</th><td>Identified person candidates for that face (ranked by confidence). Array size should be no larger than input maxNumOfCandidatesReturned. If no person is identified, will return an empty array. </td></tr>
        /// <tr><td>personId</td><th>String</th><td>personId of candidate person.</td></tr>
        /// <tr><td>confidence</td><th>Number</th><td>A float number between 0.0 and 1.0.</td></tr>
        /// </tbody>
        /// </table></returns>
        System.Threading.Tasks.Task IdentifyFaceAsync(object body);

        /// <summary>Face - Verify</summary>
        /// <param name="body">JSON fields in face to face verification request body:
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
        /// 			<td>faceId1</td>
        /// 			<th>String</th>
        /// 			<td>faceId of one face, comes from
        /// 				<a href="/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236">Face - Detect</a>.</td>
        /// 		</tr>
        /// 		<tr>
        /// 			<td>faceId2</td>
        /// 			<th>String</th>
        /// 			<td>faceId of another face, comes from
        /// 				<a href="/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236">Face - Detect</a>.</td>
        /// 		</tr>
        /// 	</tbody>
        /// </table> JSON fields in face to person verification request body:
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
        /// 			<td>faceId</td>
        /// 			<th>String</th>
        /// 			<td>faceId of the face, comes from
        /// 				<a href="/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236">Face - Detect</a>.</td>
        /// 		</tr>
        /// 		<tr>
        /// 			<td>personGroupId</td>
        /// 			<th>String</th>
        /// 			<td>Using existing personGroupId and personId for fast loading a specified person. personGroupId is created in
        /// 				<a href="/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395244">PersonGroup - Create</a>. Parameter personGroupId and largePersonGroupId should not be provided at the same time.</td>
        /// 		</tr>
        /// 		<tr>
        /// 			<td>largePersonGroupId</td>
        /// 			<th>String</th>
        /// 			<td>Using existing largePersonGroupId and personId for fast loading a specified person. largePersonGroupId is created in
        /// 				<a href="/docs/services/563879b61984550e40cbbe8d/operations/599acdee6ac60f11b48b5a9d">LargePersonGroup - Create</a>. Parameter personGroupId and largePersonGroupId should not be provided at the same time.</td>
        /// 		</tr>
        /// 		<tr>
        /// 			<td>personId</td>
        /// 			<th>String</th>
        /// 			<td>Specify a certain person in a person group or a large person group. personId is created in
        /// 				<a href="/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f3039523c">PersonGroup Person - Create</a> or
        /// 				<a href="/docs/services/563879b61984550e40cbbe8d/operations/599adcba3a7b9412a4d53f40">LargePersonGroup Person - Create</a>.</td>
        /// 		</tr>
        /// 	</tbody>
        /// </table></param>
        /// <returns>A successful call returns the verification result.
        /// <br /><br /> JSON fields in response body:
        /// <table class="element table">
        /// <thead>
        /// <tr><th>Fields</th><th>Type</th><th>Description</th></tr>
        /// </thead>
        /// <tbody>
        /// <tr><td>isIdentical</td><th>Boolean</th><td>True if the two faces belong to the same person or the face belongs to the person, otherwise false.</td></tr>
        /// <tr><td>confidence</td><th>Number</th><td>A number indicates the similarity confidence of whether two faces belong to the same person, or whether the face belongs to the person. By default, isIdentical is set to True if similarity confidence is greater than or equal to 0.5. This is useful for advanced users to override "isIdentical" and fine-tune the result on their own data.</td></tr>
        /// </tbody>
        /// </table></returns>
        System.Threading.Tasks.Task VerifyFaceAsync(object body);


    }
}
