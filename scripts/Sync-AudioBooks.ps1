[CmdletBinding()]
param (
	[string]$CatalogUrl = 'http://oszkapi-dev.azurewebsites.net/api/audiobooks'
)
Clear-Host

$audioBooks = Invoke-RestMethod $CatalogUrl

foreach ($book in $audioBooks) {
	$url = $book.__metadata.mekUrl
	"$url/mp3"
	"$url/index.xml"
}


