[CmdletBinding()]
param (
	[string]$CatalogUrl = 'http://oszkapi-dev.azurewebsites.net/api/audiobooks'
)

$audioBooks = Invoke-RestMethod $CatalogUrl

foreach ($book in $audioBooks) {
	$book.__metadata
}


