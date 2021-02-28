import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { PrimaryTextbox } from "../../atoms/Textboxes";
import { LabelPrimary } from "../../atoms/Labels";

export default () => {
  return (
    <div className="row mb-2">
      <div className="col-12 col-md-6 col-lg-4">
        <div className="row">
          <div className="col-3 mt-2">
            <FontAwesomeIcon
              size="xs"
              icon="chevron-right"
              className="me-1"
            ></FontAwesomeIcon>
            <LabelPrimary className="d-none d-sm-inline">Value</LabelPrimary>
          </div>
          <div className="col-9">
            <PrimaryTextbox name="name" placeholder="Name" />
          </div>
        </div>
      </div>
      <div className="col-12 col-md-6 col-lg-4">
        <PrimaryTextbox name="priceAdjustment" placeholder="Price adjustment" />
      </div>
    </div>
  );
};
