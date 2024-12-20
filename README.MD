NmmQuad - NMM Quadrature Signal Viewer
======================================

A standalone command line tool to check the quadrature signals of the laser interferometers of a [SIOS](https://sios-de.com) NMM. The signals must be recorded in the respective scan files.

## Overview
The Nanopositioning and Nanomeasuring Machine (NMM) by [SIOS GmbH](https://sios-de.com) is a versatile metrological instrument. It can perform various dimensional measurements like profile and surface scans with different sensors. All measurement modes are traceable due to three laser interferometers. These homodyne interferometers suffer from drawbacks common to all interferometers, most notably the nonlinearity errors. This tool allows to check for the presence of such errors and the effectiveness of two different correction methods. These correction methods are implemented by [Bev.IO.NmmReader](https://github.com/matusm/Bev.IO.NmmReader), a library to load the measurement files produced by the NMM.

## Command Line Usage:  
```
NmmQuad inputfile [outputfile] [options]
```

## Options:  
`--scan (-s)` : Scan index for multi-scan files.

`--profile (-p)` : Extract a single profile. If `--profile=0` extract the whole scan field.
 
`--back` : Use the backtrace scan data only (when present).

`--corr` : Plot raw data and results of correction methods.

`--size (-g)` : Size of plot graphic (in pixels).

## Dependencies
* [Bev.IO.NmmReader](https://github.com/matusm/Bev.IO.NmmReader)  
* [At.Matus.StatisticPod](https://github.com/matusm/At.Matus.StatisticPod)
* [CommandLineParser](https://github.com/commandlineparser/commandline)
* [Math.NET numerics](https://numerics.mathdotnet.com)
