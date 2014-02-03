Report
======

*   The current status of a subject is described in a report.
*   It is created by a business logic (main.ps1).
*   It is an instance of [System.Collections.Hashtable](http://msdn.microsoft.com/en-us/library/system.collections.hashtable(v=vs.100).aspx).
*   It has the following entries.
<table>
    <tr><th>Key             </th><th>Value type  </th><th>Value                                         </th><th>Example</th></tr>
    <tr><td>Title           </td><td>string </td><td>Title of the report                                </td><td>"FAILED"</td></tr>
    <tr><td>Subject         </td><td>string </td><td>Subject of investigation                           </td><td>"Job001"</td></tr>
    <tr><td>CurrentStatus   </td><td>string </td><td>Current status of the subject                      </td><td>"failed"</td></tr>
    <tr><td>CurrentGrade    </td><td>double </td><td>Current grade (+2, +1, 0, -1, -2) of the subject   </td><td>-1</td></tr>
    <tr><td>IsUpdated       </td><td>bool   </td><td>The report is updated or not                       </td><td>True</td></tr>
    <tr><td>SourceUrl       </td><td>string </td><td>The source of the report                           </td><td>"http://localhost:8080/job/Job001/"</td></tr>
</table>
