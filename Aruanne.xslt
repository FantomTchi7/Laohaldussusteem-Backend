<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" indent="yes"/>
  
  <xsl:param name="searchTerm" />

  <xsl:template match="/">
    <html>
      <head>
        <title>Detailne Laoseisu Aruanne</title>
        <style>
          body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; padding: 20px; background-color: #f9f9f9; }
          h1 { color: #2c3e50; }
          
          .top-bar { display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px; }
          
          /* Search Box Styles */
          .search-container { background: white; padding: 15px; border-radius: 8px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); display: inline-flex; gap: 10px; }
          .search-input { padding: 8px; border: 1px solid #ddd; border-radius: 4px; width: 250px; }
          .btn { padding: 8px 15px; border: none; border-radius: 4px; cursor: pointer; color: white; text-decoration: none; font-size: 14px; }
          .btn-search { background-color: #007bff; }
          .btn-reset { background-color: #6c757d; display: inline-block; padding: 8px 15px; }

          .warehouse-block { margin-bottom: 40px; background: white; padding: 20px; border-radius: 8px; box-shadow: 0 2px 5px rgba(0,0,0,0.1); }
          .ladu-title { font-size: 1.2em; color: #fff; background-color: #2c3e50; padding: 10px; border-radius: 4px; margin-bottom: 10px; }
          
          table { border-collapse: collapse; width: 100%; font-size: 0.9em; }
          th, td { border: 1px solid #e0e0e0; padding: 8px; text-align: left; }
          th { background-color: #f2f2f2; font-weight: 600; }
          
          .arve-meta { background-color: #e8f6f3; color: #333; }
          .money { text-align: right; font-family: monospace; }
        </style>
      </head>
      <body>
        <div class="top-bar">
            <div>
                <h1>Aruanne: Laoseis ja Arved</h1>
            </div>
            
            <div class="search-container">
                <form method="get" action="html" style="margin:0; display:flex; gap:5px;">
                    <input type="text" name="search" class="search-input" placeholder="Otsi tellijat või arve nr..." value="{$searchTerm}" />
                    <button type="submit" class="btn btn-search">Otsi</button>
                    <xsl:if test="$searchTerm != ''">
                        <a href="html" class="btn btn-reset">Tühista</a>
                    </xsl:if>
                </form>
            </div>
        </div>
        
        <xsl:if test="count(laoseis/ladu) = 0">
            <div style="text-align:center; padding: 50px; color: #666;">
                <h3>Vasteid ei leitud.</h3>
            </div>
        </xsl:if>

        <xsl:for-each select="laoseis/ladu">
          <div class="warehouse-block">
            <div class="ladu-title">
              Ladu: <xsl:value-of select="@aadress"/> (ID: <xsl:value-of select="@id"/>)
            </div>
            
            <table>
              <thead>
                <tr>
                  <th>Toode</th>
                  <th>Kogus</th>
                  <th>Ühik</th>
                  <th>Hind</th>
                  <th>Rida Summa</th>
                  <th>Arve Nr</th>
                  <th>Klient</th>
                  <th>Ettevõte</th>
                  <th>Tähtaeg</th>
                  <th>Koostaja</th>
                  <th>Arve Summa (KM-ga)</th>
                </tr>
              </thead>
              <tbody>
                <xsl:for-each select="arve">
                  <xsl:variable name="arveNr" select="@arve_nr"/>
                  <xsl:variable name="klient" select="@tellija"/>
                  <xsl:variable name="ettevote" select="@ettevote"/>
                  <xsl:variable name="tahtaeg" select="@maksetahtaeg"/>
                  <xsl:variable name="koostaja" select="@koostaja"/>
                  <xsl:variable name="bruto" select="@summa_bruto"/>
                  
                  <xsl:for-each select="toode">
                    <tr>
                      <td><xsl:value-of select="nimetus"/></td>
                      <td><xsl:value-of select="kogus"/></td>
                      <td><xsl:value-of select="uhik"/></td>
                      <td class="money"><xsl:value-of select="hind"/> €</td>
                      <td class="money"><xsl:value-of select="rida_summa"/> €</td>
                      
                      <td class="arve-meta"><xsl:value-of select="$arveNr"/></td>
                      <td class="arve-meta"><xsl:value-of select="$klient"/></td>
                      <td class="arve-meta"><xsl:value-of select="$ettevote"/></td>
                      <td class="arve-meta"><xsl:value-of select="$tahtaeg"/></td>
                      <td class="arve-meta"><xsl:value-of select="$koostaja"/></td>
                      <td class="arve-meta money" style="font-weight:bold;"><xsl:value-of select="$bruto"/> €</td>
                    </tr>
                  </xsl:for-each>
                </xsl:for-each>
              </tbody>
            </table>
          </div>
        </xsl:for-each>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>