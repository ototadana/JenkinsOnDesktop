レポート
========

*   レポートには調査対象の現在の状態が記述されています。
*   レポートはビジネスロジック (main.ps1) が作成します。
*   レポートは [System.Collections.Hashtable](http://msdn.microsoft.com/en-us/library/system.collections.hashtable(v=vs.100).aspx) のインスタンスです。
*   レポートには以下のエントリーがあります。
<table>
    <tr><th>キー            </th><th>値の型 </th><th>値                                                 </th><th>値の例</th></tr>
    <tr><td>Title           </td><td>string </td><td>レポートのタイトル                                 </td><td>"FAILED"</td></tr>
    <tr><td>Subject         </td><td>string </td><td>調査対象                                           </td><td>"Job001"</td></tr>
    <tr><td>CurrentStatus   </td><td>string </td><td>調査対象の現在の状態                               </td><td>"failed"</td></tr>
    <tr><td>CurrentGrade    </td><td>double </td><td>調査対象の現在の状態を数値評価したもの (+2, +1, 0, -1, -2) </td><td>-1</td></tr>
    <tr><td>IsUpdated       </td><td>bool   </td><td>このレポートが前回から変更されたかどうか           </td><td>True</td></tr>
    <tr><td>SourceUrl       </td><td>string </td><td>レポートの情報源                                   </td><td>"http://localhost:8080/job/Job001/"</td></tr>
</table>
